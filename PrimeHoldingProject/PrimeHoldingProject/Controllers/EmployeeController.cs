using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeHoldingProject.Core.Contracts;
using PrimeHoldingProject.Core.Models.Employee;
using PrimeHoldingProject.Core.Models.User;
using PrimeHoldingProject.UserServices;
using System.Security.Claims;
using static PrimeHoldingProject.Infrastructure.Constants.RoleConstants;
using static PrimeHoldingProject.Core.Constants.MessageConstant;


namespace PrimeHoldingProject.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUserService userService;
        private readonly IEmployeeService employeeService;
        public EmployeeController(IUserService userService,
            IEmployeeService employeeService)
        {
            this.userService = userService;
            this.employeeService = employeeService;
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
            }
            catch (ArgumentException)
            {
                ModelState.AddModelError("", "Invalid user!");
                return View(model);
            }
            return RedirectToAction("Index", "Home");
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


        private string GetUserId()
        {
            return User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
