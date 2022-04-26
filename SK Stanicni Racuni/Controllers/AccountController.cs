using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SK_Stanicni_Racuni.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext context;
        private readonly INotyfService notyf;

        public AccountController(AppDbContext context, INotyfService notyf)
        {
            this.context = context;
            this.notyf = notyf;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        public async Task<IActionResult> Log(UserTab userTab)
        {

            if (ModelState.IsValid)
            {
                var user = context.UserTabs.Where(x => x.UserId == userTab.UserId && x.Lozinka == userTab.Lozinka).FirstOrDefault();

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    notyf.Error("Pogrešan korisnički ID ili lozinka.");
                    return RedirectToAction("Login");
                }


                if (user.Stanica.StartsWith("000") && !user.Stanica.StartsWith("00099"))
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    notyf.Error("Nemate prava pristupa.");
                    return RedirectToAction("Login");
                }

                var claims = (dynamic)null;

                if (!user.Stanica.StartsWith("00099"))
                {
                    claims = new List<Claim>
                        {
                            new Claim("UserID", user.UserId),
                            new Claim("Lozinka", user.Lozinka),
                            new Claim(ClaimTypes.Name, user.UserId),
                            new Claim("Naziv",  user.Naziv.Trim() + " "+ user.Grupa.Trim())
                        };

                } else
                {
                    claims = new List<Claim>
                        {
                            new Claim("UserID", user.UserId),
                            new Claim("Lozinka", user.Lozinka),
                            new Claim(ClaimTypes.Name, user.UserId),
                            new Claim("Naziv",  user.Naziv)
                        };
                }

                var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {

                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            }

            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index", "home");
        }
    }
}


//za detekciju korisnika i da li je ulogovan
//bool val1 = HttpContext.User.Identity.IsAuthenticated;
//var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
//var userName = httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Name); // will give the user's userName
////var userName = User.FindFirstValue(ClaimTypes("UserID"); // will give the user's userName
//var username = HttpContext.User.Identity.Name;