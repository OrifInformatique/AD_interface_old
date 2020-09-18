using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Properties;

namespace WebApplication1.Controllers
{
    public class CheckController : Controller
    {
        /// <summary>
        /// Displays a list of users that are not part of both the `SAIUSER` AD group and not in any of the GGDAC groups
        /// </summary>
        /// <returns></returns>
        public ActionResult SAIGroup()
        {
            List<Principal> users = new List<Principal>();
            GroupPrincipal saiUserGroup = GroupPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain, Settings.Default.ADPath), "SAIUSER");

            string[] saiGroups = new string[]
            {
                "GGDAC-10-SaiLecteur",
                "GGDAC-20-SaiMSPEDU",
                "GGDAC-50-SaiAdministration",
                "GGDAC-70-SaiDirection",
                "GGDAC-80-SaiCoordinateurs",
                "GGDAC-90-SaiCoordinateurGlobal"
            };
            foreach (string saiGroup in saiGroups)
            {
                GroupPrincipal group = GroupPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain, Settings.Default.ADPath), saiGroup);
                if (group is null)
                {
                    continue;
                }

                foreach (Principal member in group.Members)
                {
                    if (member is GroupPrincipal)
                    {
                        if (!member.IsMemberOf(saiUserGroup))
                        {
                            users.Add(member);
                        }
                    }
                }
                foreach (Principal member in saiUserGroup.Members)
                {
                    if (member is GroupPrincipal)
                    {
                        if (!member.IsMemberOf(group))
                        {
                            users.Add(member);
                        }
                    }
                }
            }
            ViewBag.Users = users.ToArray();
            return View();
        }

        /// <summary>
        /// Displays a list of users that share the same phone number
        /// </summary>
        /// <returns></returns>
        public ActionResult DuplicatePhone()
        {
            Dictionary<string, UserModel> phones = new Dictionary<string, UserModel>();
            SortedDictionary<string, List<UserModel>> users = new SortedDictionary<string, List<UserModel>>();
            DirectorySearcher adSearcher = new DirectorySearcher(new DirectoryEntry("LDAP://" + Settings.Default.ADPath));
            adSearcher.Filter = "(&(telephoneNumber=*))";
            SearchResultCollection coll = adSearcher.FindAll();
            foreach (SearchResult item in coll)
            {
                UserModel currentUser = new UserModel(item.GetDirectoryEntry());
                string phoneNumber = currentUser.PropertyToString("telephoneNumber");
                if (phones.ContainsKey(phoneNumber))
                {
                    if (users.ContainsKey(phoneNumber))
                    {
                        users[phoneNumber].Add(currentUser);
                    }
                    else
                    {
                        users.Add(phoneNumber, new List<UserModel>() { phones[phoneNumber], currentUser });
                    }
                }
                else
                {
                    phones.Add(phoneNumber, currentUser);
                }
            }
            ViewBag.Users = users;
            return View();
        }

        public ActionResult GroupsDescription()
        {
            List<GroupModel> groups = new List<GroupModel>();
            DirectorySearcher adSearcher = new DirectorySearcher(new DirectoryEntry("LDAP://" + Settings.Default.ADPath));
            adSearcher.Filter = "(&(!description=*)(objectCategory=group))";
            SearchResultCollection coll = adSearcher.FindAll();
            foreach (SearchResult item in coll)
            {
                groups.Add(new GroupModel(item.GetDirectoryEntry()));
            }
            ViewBag.Groups = groups;
            return View();
        }
    }
}
