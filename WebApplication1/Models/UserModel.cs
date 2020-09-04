using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using WebApplication1.Properties;

namespace WebApplication1.Models
{
    public class UserModel
    {
        public SearchResult Result;

        public Dictionary<string, string> Properties = new Dictionary<string, string>()
        {
            { "samAccountName", null },
            { "givenName", null },
            { "sn", null },
            { "displayName", null },
            { "description", null },
            { "physicalDeliveryOfficeName", null },
            { "telephoneNumber", null },
            { "mail", null },
            { "l", null },
            { "st", null },
            { "postalCode", null },
            { "co", null },
            { "userPrincipalName", null },
            { "accountExpires", null },
            { "homeDirectory", null },
            { "ipPhone", null },
            { "title", null },
            { "department", null },
            { "company", null },
            { "extensionAttribute1", null },
            { "memberOf", null },
            { "StreetAddress", null },
            { "homePhone", null },
            { "pager", null },
            { "mobile", null },
            { "facsimileTelephoneNumber", null },
            { "DirectReports", null },
            { "manager", null },
            { "cn", null }
        };

        public UserModel(SearchResult result)
        {
            Result = result;
            foreach (string property in Properties.Keys.ToArray())
            {
                foreach (var obj in Result.Properties[property])
                {
                    if (!string.IsNullOrEmpty(Properties[property]))
                    {
                        Properties[property] += ", ";
                    }
                    Properties[property] += obj.ToString();
                }
            }
        }

        public string GetAccountExpires()
        {
            // the values 0 and (2^63 - 1) mean "Never"
            return (Properties["accountExpires"] == "0" || Properties["accountExpires"] == "9223372036854775807") ? "Never" : Properties["accountExpires"]; // 0x7FFFFFFFFFFFFFFF
        }

        public UserModel GetManager()
        {
            if (!string.IsNullOrWhiteSpace(Properties["manager"]))
            {
                DirectorySearcher adSearcher = new DirectorySearcher(new DirectoryEntry("LDAP://" + Settings.Default.ADPath));
                adSearcher.Filter = ("(&(distinguishedName=" + Properties["manager"] + "))");
                SearchResult result = adSearcher.FindOne();
                if (result != null)
                {
                    return new UserModel(result);
                }
            }
            return null;
        }

        public UserModel[] GetDirectReports()
        {
            List<UserModel> reports = new List<UserModel>();
            foreach (string property in Result.Properties["DirectReports"])
            {
                DirectorySearcher adSearcher = new DirectorySearcher(new DirectoryEntry("LDAP://" + Settings.Default.ADPath));
                adSearcher.Filter = ("(&(distinguishedName=" + property + "))");
                SearchResult result = adSearcher.FindOne();
                if (result != null)
                {
                    reports.Add(new UserModel(result));
                }
            }
            return reports.ToArray();
        }

        public DirectoryEntry[] GetGroups()
        {
            List<DirectoryEntry> groups = new List<DirectoryEntry>();
            object obGroups = Result.GetDirectoryEntry().Invoke("Groups");
            foreach (object ob in (IEnumerable)obGroups)
            {
                DirectoryEntry obGpEntry = new DirectoryEntry(ob);
                groups.Add(obGpEntry);
            }
            return groups.ToArray();
        }

        public GroupModel[] GetGroupsRecursively()
        {
            List<GroupModel> groups = new List<GroupModel>();
            foreach (DirectoryEntry de in GetGroups())
            {
                GroupModel groupModel = new GroupModel(de);
                groupModel.GetChildrensRecursively();
                groups.Add(groupModel);
            }
            return groups.ToArray();
        }
    }
}