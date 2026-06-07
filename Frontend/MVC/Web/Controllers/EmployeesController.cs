using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IApiClient _apiClient;

        public EmployeesController(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var employees = await _apiClient.GetEmployeesAsync();
            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _apiClient.GetEmployeeAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                await _apiClient.CreateEmployeeAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _apiClient.GetEmployeeAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeViewModel employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _apiClient.UpdateEmployeeAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _apiClient.GetEmployeeAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiClient.DeleteEmployeeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Employees/ComputePay
        public async Task<IActionResult> ComputePay()
        {
            var employees = await _apiClient.GetEmployeesAsync();
            return View(employees);
        }

        // POST: Employees/ComputePay
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ComputePay(int employeeId)
        {
            try
            {
                var pay = await _apiClient.ComputePayAsync(employeeId);
                ViewBag.ComputedPay = pay;
                ViewBag.EmployeeId = employeeId;
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP errors (like 500 Internal Server Error)
                ViewBag.ErrorMessage = $"Error calculating pay: {ex.Message}";
                ViewBag.ErrorStatusCode = ((int)ex?.StatusCode).ToString();
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
            }

            var employees = await _apiClient.GetEmployeesAsync();
            return View(employees);
        }
    }
}