using System.Collections.Generic;
using System.DirectoryServices;

namespace WebApplication1.Models
{
    public class GroupModel
    {
        public List<GroupModel> childrens = new List<GroupModel>();
        public DirectoryEntry currentGroup;
        public bool notLoadedGroups = false;

        public GroupModel(DirectoryEntry currentGroup)
        {
            this.currentGroup = currentGroup;
        }

        public void GetChildrensRecursively(int count = 0)
        {
            for (int i = 0; i < currentGroup.Properties["memberOf"].Count; i++)
            {
                DirectorySearcher adSearcher = new DirectorySearcher(new DirectoryEntry("LDAP://orif.lan"));
                adSearcher.Filter = ("(&(distinguishedName=" + currentGroup.Properties["memberOf"][i] + "))");
                SearchResult result = adSearcher.FindOne();
                if (result != null)
                {
                    childrens.Add(new GroupModel(result.GetDirectoryEntry()));
                }
            }
            foreach (GroupModel ch in childrens)
            {
                if (count < 10)
                {
                    ch.GetChildrensRecursively(count + 1);
                }
                else
                {
                    ch.notLoadedGroups = true;
                }
            }
        }
    }
}