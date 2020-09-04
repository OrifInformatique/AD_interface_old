using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Properties;

namespace WebApplication1.Controllers
{
    public class InfoController : Controller
    {
        public ActionResult Find(string username)
        {
            List<UserModel> users = new List<UserModel>();
            if (!string.IsNullOrWhiteSpace(username))
            {
                DirectorySearcher adSearcher = new DirectorySearcher(new DirectoryEntry("LDAP://" + Settings.Default.ADPath));
                adSearcher.Filter = ("(&(samAccountName=" + username + ")(objectCategory=person))");
                try
                {
                    SearchResultCollection coll = adSearcher.FindAll();
                    foreach (SearchResult item in coll)
                    {
                        users.Add(new UserModel(item.GetDirectoryEntry()));
                    }
                }
                catch (System.ArgumentException)
                {

                }
            }
            ViewBag.Users = users;
            return View("Find");
        }

        public ActionResult Detail(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                DirectorySearcher adSearcher = new DirectorySearcher(new DirectoryEntry("LDAP://" + Settings.Default.ADPath));
                adSearcher.Filter = ("(&(samAccountName=" + id + ")(objectCategory=person))");
                try
                {
                    SearchResult result = adSearcher.FindOne();
                    if (result != null)
                    {
                        ViewBag.User = new UserModel(result);
                        return View();
                    }
                }
                catch (System.ArgumentException)
                {

                }
            }
            return RedirectToAction("Find");
        }

        public ActionResult Group(string id)
        {
            List<Principal> users = new List<Principal>();
            if (!string.IsNullOrEmpty(id))
            {
                //DirectorySearcher adSearcher = new DirectorySearcher(new DirectoryEntry("LDAP://" + Settings.Default.ADPath));
                //adSearcher.Filter = ("(&(samAccountName=" + id + ")(objectCategory=group))");
                GroupPrincipal group = GroupPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain, Settings.Default.ADPath), id);
                /*try
                {
                    SearchResult result = adSearcher.FindOne();
                    if (result != null)
                    {*/
                        var results = group.GetMembers(true);
                        ViewBag.group = group;
                        foreach (var result in results)
                        {
                            users.Add(result);
                        }
                        ViewBag.users = users.ToArray();
                        return View();
                    /*}
                }
                catch (System.ArgumentException)
                {

                }*/
            }
            return RedirectToAction("Find");
        }

        public ActionResult AjaxSubGroups(string distinguishedName)
        {
            List<DirectoryEntry> groups = new List<DirectoryEntry>();
            if (!string.IsNullOrWhiteSpace(distinguishedName))
            {
                DirectorySearcher adSearcher = new DirectorySearcher(new DirectoryEntry("LDAP://" + Settings.Default.ADPath));
                adSearcher.Filter = ("(&(distinguishedName=" + distinguishedName + "))");
                SearchResult result = adSearcher.FindOne();
                if (result != null)
                {
                    DirectoryEntry subGroups = result.GetDirectoryEntry();
                    for (int i = 0; i < subGroups.Properties["memberOf"].Count; i++)
                    {
                        DirectorySearcher adSearcher2 = new DirectorySearcher(new DirectoryEntry("LDAP://" + Settings.Default.ADPath));
                        adSearcher2.Filter = ("(&(distinguishedName=" + subGroups.Properties["memberOf"][i] + "))");
                        SearchResult result2 = adSearcher2.FindOne();
                        if (result2 != null)
                        {
                            groups.Add(result2.GetDirectoryEntry());
                        }
                    }
                }
            }
            ViewBag.groups = groups.ToArray();
            return View();
        }
    }
}