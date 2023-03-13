using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrimeHoldingProject.Core.Contracts;
using PrimeHoldingProject.Core.Models.Task;
using PrimeHoldingProject.Core.Models.User;
using PrimeHoldingProject.Infrastructure.Data.Models;
using PrimeHoldingProject.UserServices;
using System.Security.Claims;
using static PrimeHoldingProject.Core.Constants.MessageConstant;
using static PrimeHoldingProject.Infrastructure.Constants.RoleConstants;

namespace PrimeHoldingProject.Controllers
{
    [Authorize]
    public class ManagerController : Controller
    {
        private readonly IUserService userService;
        private readonly IManagerService managerService;
        private readonly ITaskService taskService;
        private readonly IEmployeeService employeeService;
        private readonly SignInManager<ApplicationUser> signInManager;
        public ManagerController(IUserService userService,
            IManagerService managerService,
            ITaskService taskService,
            IEmployeeService employeeService,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userService = userService;
            this.managerService = managerService;
            this.taskService = taskService;
            this.employeeService = employeeService;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> Become()
        {
            try
            {
                if (User.IsInRole(ManagerConstant) || User.IsInRole(EmployeeConstant))
                {
                    TempData[ErrorMessage] = "You are already an employee/manager!";
                    return RedirectToAction("Index", "Home");
                }
                var userId = GetUserId();
                var model = await userService.GetUserManagerInfoAsync(Guid.Parse(userId));
                return View(model);
            }
            catch (ArgumentException)
            {
                TempData[ErrorMessage] = "Invalid user!";
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Become(UserManagerViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Invalid info.");
                    return View();
                }
                var userId = GetUserId();
                await managerService.BecomeManagerAsync(model, Guid.Parse(userId));
                TempData[SuccessMessage] = "You are now a manager! Please login to confirm changes!";
                await signInManager.SignOutAsync();
                return RedirectToAction("Login", "User");
            }
            catch (ArgumentException)
            {
                TempData[ErrorMessage] = "Something went wrong!";
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpGet]
        [Authorize(Roles = ManagerConstant)]
        public async Task<IActionResult> Edit()
        {
            try
            {
                var userId = GetUserId();
                var user = await managerService.GetManagerInformation(Guid.Parse(userId));
                return View(user);
            }
            catch (ArgumentException)
            {
                TempData[ErrorMessage] = "An error occured!";
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserManagerViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Invalid info.");
                    return View();
                }
                var userId = GetUserId();
                await managerService.EditManager(model, Guid.Parse(userId));
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
        [Authorize(Roles = ManagerConstant)]
        public async Task<IActionResult> QuitJob()
        {
            try
            {
                await managerService.QuitJobAsync(Guid.Parse(GetUserId()));
                await signInManager.SignOutAsync();
                TempData[SuccessMessage] = "You succesfully left work! Please log in to confirm changes!";
                return RedirectToAction("Login", "User");
            }
            catch (ArgumentNullException)
            {
                TempData[ErrorMessage] = "Invalid manager!";
                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException)
            {
                TempData[ErrorMessage] = "Invalid user!";
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        [Authorize(Roles = ManagerConstant)]
        public async Task<IActionResult> AllEmployees()
        {
            var employees = await employeeService.AllEmployeesAsync();
            return View(employees);
        }
        [Authorize(Roles = ManagerConstant)]
        public async Task<IActionResult> FireEmployee(Guid employeeId)
        {
            try
            {
                await managerService.FireEmployeeAsync(employeeId);
                TempData[SuccessMessage] = "You successfully fired an employee!";
                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException)
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
