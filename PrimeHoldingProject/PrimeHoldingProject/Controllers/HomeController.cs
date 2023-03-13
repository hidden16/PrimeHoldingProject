using Microsoft.AspNetCore.Mvc;
using PrimeHoldingProject.Core.Contracts;

namespace PrimeHoldingProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeService employeeService;

        public HomeController(ILogger<HomeController> logger,
            IEmployeeService employeeService)
        {
            _logger = logger;
            this.employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await employeeService.EmployeeRanklistAsync();
            return View(employees);
        }
    }
}