using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTOs;
using Api.Models;

namespace Api.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto> GetEmployeeByIdAsync(int id);
        Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createDto);
        Task<EmployeeDto> UpdateEmployeeAsync(UpdateEmployeeDto updateDto);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<decimal> ComputePayAsync(int employeeId);
    }
}