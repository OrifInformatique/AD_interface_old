﻿using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Web.Mvc;
using WebApplication1.Helpers;
using WebApplication1.Properties;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string username = "", string password = "")
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                bool isValid = new PrincipalContext(ContextType.Domain, Settings.Default.ADPath).ValidateCredentials(username, password);
                if (isValid)
                {
                    SqlConnection conn = DBUtils.GetDBConnection();
                    conn.Open();
                    try
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "SELECT * FROM users WHERE";
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                ViewBag.accessLevel = reader.GetInt32(reader.GetOrdinal("access_level"));
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                    return View("User");
                }
            }
            return View();
        }
    }
}