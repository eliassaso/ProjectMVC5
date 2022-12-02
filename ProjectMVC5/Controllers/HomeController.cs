using ProjectMVC5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ProjectMVC5.Controllers
{
    public class HomeController : Controller
    {
        private DB_Entities _db = new DB_Entities();
        // GET: Home
        public ActionResult Index()
        {
            if (Session["idUser"] != null)
            {
                List<User> users = _db.Users.ToList();
                return View(users);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //GET: Register

        public ActionResult Register()
        {
            ViewBag.Roles = selectList();
            return View();
        }

        private List<SelectListItem> selectList()
        {
            var _selectList = new List<SelectListItem>();
            foreach (var item in _db.Roles.ToList())
            {
                _selectList.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.role.ToString()
                });
            }
            return _selectList;

        }

        //POST: Register
        // comentario ejemplo para git
        // otro comentario
        // otro comentario segunda pantalla
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                var check = _db.Users.FirstOrDefault(s => s.Email == _user.Email);
                
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.Users.Add(_user);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Roles = selectList();
                    ViewBag.error = "Email already exists";
                    return View();
                }


            }
            ViewBag.Roles = selectList();
            return View();


        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult AjaxNetTest(String email)
        {
            String idUser = _db.Users.Where(s => s.Email.Equals(email)).ToList().FirstOrDefault().idUser.ToString();
            return Content("The User id: " + idUser);
        }

        public ActionResult Login2(String email, String password)
        {
            String response = "Login failed";
            // return Content($"Email: {email} Password: {password}");
            var f_password = GetMD5(password);
            User user = _db.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList().FirstOrDefault();
            //var association = _db.AsociacionPerfil.Where(a => a.idUser == user.idUser).FirstOrDefault();

            if (user != null)
            {
                response = "You are login as: " + user.FirstName;
                var roles = _db.Roles.Where(p => p.Id == user.Perfil).FirstOrDefault();
                Session["FullName"] = user.FirstName + " " + user.LastName;
                Session["Email"] = user.Email;
                Session["idUser"] = user.idUser;
                Session["Roles"] = roles.role;
                return RedirectToAction("ExampleView", user);
            }
            return Content(response);
        }

        public ActionResult ExampleView(User user)
        {
            if (Session["idUser"] != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Login", user);
            }
        }

        public ActionResult DataValidation(String email, String password)
        {
            String response = "Login failed";
            // return Content($"Email: {email} Password: {password}");
            var f_password = GetMD5(password);
            var user = _db.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList().FirstOrDefault();
            //var association = _db.AsociacionPerfil.Where(a => a.idUser == user.idUser).FirstOrDefault();

            if (user != null)
            {
                response = "Success";
                var roles = _db.Roles.Where(p => p.Id == user.Perfil).FirstOrDefault();
                Session["FullName"] = user.FirstName + " " + user.LastName;
                Session["Email"] = user.Email;
                Session["idUser"] = user.idUser;
                Session["Roles"] = roles.role;
            }
            return Content(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin _userLogin)
        {
            if (ModelState.IsValid)
            {


                var f_password = GetMD5(_userLogin.Password);
                var user = _db.Users.Where(s => s.Email.Equals(_userLogin.Email) && s.Password.Equals(f_password)).ToList().FirstOrDefault();
                //var association = _db.AsociacionPerfil.Where(a => a.idUser == user.idUser).FirstOrDefault();
                
                if (user != null)
                {
                    //add session
                    var roles = _db.Roles.Where(p => p.Id == user.Perfil).FirstOrDefault();
                    Session["FullName"] = user.FirstName + " " + user.LastName;
                    Session["Email"] = user.Email;
                    Session["idUser"] = user.idUser; 
                    Session["Roles"] = roles.role;
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return View();
                }
            }
            return View();
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }



        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

    }
}