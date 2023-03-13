using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeHoldingProject.Core.Contracts;
using PrimeHoldingProject.Core.Models.Task;
using System.Security.Claims;
using static PrimeHoldingProject.Core.Constants.MessageConstant;
using static PrimeHoldingProject.Infrastructure.Constants.RoleConstants;

namespace PrimeHoldingProject.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskService taskService;
        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var tasks = await taskService.AllTasksAsync();
            return View(tasks);
        }
        [HttpGet]
        [Authorize(Roles = ManagerConstant)]
        public async Task<IActionResult> AllForManagers()
        {
            var tasks = await taskService.AllTasksForManagersAsync();
            return View(tasks);
        }
        public async Task<IActionResult> FinishTask(Guid taskId)
        {
            try
            {
                if (User.IsInRole(EmployeeConstant))
                {
                    await taskService.FinishTaskAsync(taskId, Guid.Parse(GetUserId()));
                    TempData[SuccessMessage] = "You successfully completed a task!";
                    return RedirectToAction(nameof(All));
                }
                TempData[ErrorMessage] = "You are not an employee!";
                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException)
            {
                TempData[ErrorMessage] = "Something went wrong!";
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        [Authorize(Roles = ManagerConstant)]
        public IActionResult CreateTask()
        {
            return View(new CreateTaskViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Invalid info.");
                    return View();
                }
                await taskService.CreateTaskAsync(model, Guid.Parse(GetUserId()));
                TempData[SuccessMessage] = "Successfully added a task!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "An error occured!";
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        [Authorize(Roles = ManagerConstant)]
        public async Task<IActionResult> Edit(Guid taskId)
        {
            try
            {
                var task = await taskService.GetTaskForEditAsync(taskId);
                return View(task);
            }
            catch (ArgumentException)
            {
                TempData[ErrorMessage] = "Something went wrong!";
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TaskForEditViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Invalid info.");
                    return View();
                }
                await taskService.EditTaskAsync(model);
                TempData[SuccessMessage] = "You successfully edited the task!";
                return RedirectToAction(nameof(All));
            }
            catch (ArgumentException)
            {
                TempData[ErrorMessage] = "Something went wrong!";
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize(Roles = ManagerConstant)]
        public async Task<IActionResult> Delete(Guid taskId)
        {
            try
            {
                await taskService.DeleteAsync(taskId);
                TempData[SuccessMessage] = "You successfully deleted the task!";
                return RedirectToAction(nameof(All));
            }
            catch (ArgumentNullException)
            {
                TempData[ErrorMessage] = "You couldn't delete the task!";
                return RedirectToAction("Index", "Home");
            }
        }
        private string GetUserId()
        {
            return User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
