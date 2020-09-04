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
        public ActionResult Index()
        {
            return View();
        }

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
    }
}