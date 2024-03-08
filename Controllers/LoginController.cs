using Epicotel.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Epicotel.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Login login)
        {
            if (ModelState.IsValid) 
            {
                if (login.Username == "admin" && login.Password == "admin")
                {
                    FormsAuthentication.SetAuthCookie(login.Username, true);
                    return RedirectToAction("Index", "Home");
                }
                TempData["ErrorLogin"] = true;
                return View(login);
            }
            else
            {
                TempData["ErrorLogin"] = true;
                return View(login);
            }           
        }

        [Authorize, HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

    }
}