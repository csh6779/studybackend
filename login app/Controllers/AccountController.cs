using Microsoft.AspNetCore.Mvc;
using login_app.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace login_app.Controllers
{
    public class AccountController : Controller
    {
        private static List<User> users = new List<User>();

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                // ✅ 비밀번호를 해시로 저장
                model.password = ComputeSha256Hash(model.password);
                users.Add(model);
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // ✅ SHA256 해시 함수
        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string ID, string password)
        {
            // ✅ 비밀번호도 해시로 변환하여 비교
            string hashedPassword = ComputeSha256Hash(password);
            var user = users.Find(u => u.ID == ID && u.password == hashedPassword);

            if (user != null)
            {
                HttpContext.Session.SetString("UserID", user.ID);
                HttpContext.Session.SetString("UserName", user.name);
                return RedirectToAction("Welcome");
            }

            ViewBag.Message = "아이디 또는 비밀번호가 잘못되었습니다.";
            return View();
        }

        [HttpGet]
        public IActionResult Welcome()
        {
            var userId = HttpContext.Session.GetString("UserID");
            var userName = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login");
            }

            ViewBag.UserName = userName;
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
