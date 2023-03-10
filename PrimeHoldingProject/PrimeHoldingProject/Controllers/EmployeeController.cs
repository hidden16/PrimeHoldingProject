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
                if (User.IsInRole(EmployeeConstant))
                {
                    TempData[ErrorMessage] = "You are already an employee!";
                    return RedirectToAction("Index", "Home");
                }
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
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
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                await employeeService.BecomeEmployeeAsync(model, Guid.Parse(userId));
            }
            catch (ArgumentException)
            {
                ModelState.AddModelError("", "Invalid user!");
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
