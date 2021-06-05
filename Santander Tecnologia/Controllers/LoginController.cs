using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Santander_Tecnologia.Data;
using Santander_Tecnologia.Models;

namespace Santander_Tecnologia.Controllers
{
    public class LoginController : Controller
    {
        private readonly Santander_TecnologiaContext _context;

        public LoginController(Santander_TecnologiaContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UserName, Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var validUser = await _context.User
                    .FirstOrDefaultAsync(m => m.UserName == user.UserName && m.Password == user.Password) != null;

                if(validUser)
                {
                    return RedirectToAction("Index","MainMenu");
                }
                else
                {
                    ViewBag.ErrorMessage = "User not found";
                    return View();
                }
            }
            return View(user);
        }
    }
}
