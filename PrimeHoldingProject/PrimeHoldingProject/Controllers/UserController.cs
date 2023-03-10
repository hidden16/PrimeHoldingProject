using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrimeHoldingProject.Core.Models.User;
using PrimeHoldingProject.Infrastructure.Data.Models;
using Ganss.XSS;

namespace PrimeHoldingProject.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private HtmlSanitizer sanitizer = new HtmlSanitizer();
        public UserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var userResult = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (userResult.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid login");

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid info.");
                return View();
            }
            var emailCheck = await userManager.FindByEmailAsync(model.Email);

            if (emailCheck != null)
            {
                ModelState.AddModelError("", "User with that email already exists.");
                return View(model);
            }

            var user = new ApplicationUser()
            {
                Email = Sanitize(model.Email),
                FirstName = Sanitize(model.FirstName),
                LastName = Sanitize(model.LastName),
                BirthDate = model.BirthDate,
                UserName = $"{model.FirstName}{model.LastName}",
                PhoneNumber = model.PhoneNumber
            };

            var createResult = await userManager.CreateAsync(user, model.Password);
            if (createResult.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }
            foreach (var error in createResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        private string Sanitize(string txt)
        {
            return sanitizer.Sanitize(txt);
        }
    }
}
