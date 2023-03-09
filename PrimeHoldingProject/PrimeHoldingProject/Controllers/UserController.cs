using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrimeHoldingProject.Infrastructure.Data.Models;

namespace PrimeHoldingProject.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult Login()
        //{
        //    if (User.Identity?.IsAuthenticated ?? false)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    var model = new LoginViewModel();
        //    return View(model);
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var user = await userManager.FindByEmailAsync(model.EmailAddress);

        //    if (user != null)
        //    {
        //        var userResult = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);

        //        if (userResult.Succeeded)
        //        {
        //            await signInManager.SignInAsync(user, isPersistent: false);
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }

        //    ModelState.AddModelError("", "Invalid login");

        //    return View(model);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult Register()
        //{
        //    if (User.Identity?.IsAuthenticated ?? false)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    var model = new RegisterViewModel();
        //    return View(model);
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.AddModelError("", "Invalid info.");
        //        return View();
        //    }

        //    return View(model);
        //}
    }
}
