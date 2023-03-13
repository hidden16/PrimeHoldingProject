using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeHoldingProject.Core.Contracts;
using PrimeHoldingProject.Core.Models.Employee;
using PrimeHoldingProject.Core.Models.User;
using PrimeHoldingProject.UserServices;
using System.Security.Claims;
using static PrimeHoldingProject.Infrastructure.Constants.RoleConstants;
using static PrimeHoldingProject.Core.Constants.MessageConstant;
using Microsoft.AspNetCore.Identity;
using PrimeHoldingProject.Infrastructure.Data.Models;

namespace PrimeHoldingProject.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUserService userService;
        private readonly IEmployeeService employeeService;
        private readonly SignInManager<ApplicationUser> signInManager;
        public EmployeeController(IUserService userService,
            IEmployeeService employeeService,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userService = userService;
            this.employeeService = employeeService;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> Become()
        {
            try
            {
                if (User.IsInRole(EmployeeConstant) || User.IsInRole(ManagerConstant))
                {
                    TempData[ErrorMessage] = "You are already an employee/manager!";
                    return RedirectToAction("Index", "Home");
                }
                var userId = GetUserId();
                var user = await userService.GetUserEmployeeInfoAsync(Guid.Parse(userId));
                return View(user);
            }
            catch (ArgumentException)
            {
                TempData[ErrorMessage] = "An error occured!";
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Become(UserEmployeeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Invalid info.");
                    return View();
                }
                var userId = GetUserId();
                await employeeService.BecomeEmployeeAsync(model, Guid.Parse(userId));
                TempData[SuccessMessage] = "You are an employee now! Please log in for changes to get saved.";
                await signInManager.SignOutAsync();
                return RedirectToAction("Login", "User");
            }
            catch (ArgumentException)
            {
                ModelState.AddModelError("", "Invalid user!");
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            try
            {
                var userId = GetUserId();
                var user = await employeeService.GetEmployeeInformation(Guid.Parse(userId));
                return View(user);
            }
            catch (ArgumentException)
            {
                TempData[ErrorMessage] = "An error occured!";
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserEmployeeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Invalid info.");
                    return View();
                }
                var userId = GetUserId();
                await employeeService.EditEmployee(model, Guid.Parse(userId));
                TempData[SuccessMessage] = "You successfully updated your information!";
                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentNullException)
            {
                TempData[ErrorMessage] = "Invalid Employee.";
                return RedirectToAction("Index", "Home");

            }
            catch (ArgumentException)
            {
                TempData[ErrorMessage] = "Invalid user.";
                return RedirectToAction("Index", "Home");

            }
        }
        public async Task<IActionResult> QuitJob()
        {
            try
            {
                await employeeService.QuitJobAsync(Guid.Parse(GetUserId()));
                await signInManager.SignOutAsync();
                TempData[SuccessMessage] = "You succesfully left work! Please log in to confirm changes!";
                return RedirectToAction("Login", "User");
            }
            catch (ArgumentNullException)
            {
                TempData[ErrorMessage] = "Invalid employee!";
                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException)
            {
                TempData[ErrorMessage] = "Invalid user!";
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> MyTasks()
        {
            try
            {
                var tasks = await employeeService.MyTasksAsync(Guid.Parse(GetUserId()));
                return View(tasks);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "Something went wrong!";
                return RedirectToAction("Index", "Home");
            }
        }


        private string GetUserId()
        {
            return User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
