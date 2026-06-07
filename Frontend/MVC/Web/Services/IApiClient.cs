using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public interface IApiClient
    {
        Task<List<EmployeeViewModel>> GetEmployeesAsync();
        Task<EmployeeViewModel> GetEmployeeAsync(int id);
        Task<EmployeeViewModel> CreateEmployeeAsync(EmployeeViewModel employee);
        Task UpdateEmployeeAsync(EmployeeViewModel employee);
        Task DeleteEmployeeAsync(int id);
        Task<decimal> ComputePayAsync(int employeeId);
    }
}