using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SampleSecureWeb.Data;
using SampleSecureWeb.Models;
using SampleSecureWeb.ViewModels;

namespace SampleSecureWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUser _userData;
        
        public AccountController(IUser user)
        {
            _userData = user;
        }

        // GET: AccountController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegistrationViewModel registrationViewModel)
        {
            try
            {
                var user = new Models.User
                {
                    Username = registrationViewModel.Username,
                    Password = registrationViewModel.Password,
                    RoleName = "contributor"
                };
                _userData.Registration(user);
                return RedirectToAction("Index", "Home");
            }
            catch (System.Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(registrationViewModel);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                loginViewModel.ReturnUrl = loginViewModel.ReturnUrl ?? Url.Content("~/");
                
                if (ModelState.IsValid)
                {
                    var user = new User
                    {
                        Username = loginViewModel.Username,
                        Password = loginViewModel.Password
                    };

                    var loginUser = _userData.Login(user); // Login method call
                    if (loginUser != null) // Check if login is successful
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Username)
                        };
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            principal,
                            new AuthenticationProperties
                            {
                                IsPersistent = loginViewModel.RememberLogin
                            });

                        // Redirect to Home after successful login
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Error = "Invalid login attempt.";
                    }
                }
            }
            catch (System.Exception ex)
            {
                ViewBag.Error = "Invalid Username or Password";
            }
            return View(loginViewModel);
        }
    }
}
