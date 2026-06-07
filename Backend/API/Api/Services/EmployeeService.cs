using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;
using Api.DTOs;
using Api.Repositories;
using Microsoft.Data.SqlClient;

namespace Api.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _repository.GetAllAsync();
            return _mapper.Map<List<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createDto)
        {
            var employee = _mapper.Map<Employee>(createDto);

            // Generate employee number
            employee.EmployeeNumber = GenerateEmployeeNumber(
                employee.LastName,
                employee.DateOfBirth);

            await _repository.AddAsync(employee);
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto> UpdateEmployeeAsync(UpdateEmployeeDto updateDto)
        {
            var employee = await _repository.GetByIdAsync(updateDto.Id);
            if (employee == null)
                return null;

            // Update fields but keep existing employee number
            employee.LastName = updateDto.LastName;
            employee.FirstName = updateDto.FirstName;
            employee.MiddleName = updateDto.MiddleName;
            employee.DateOfBirth = updateDto.DateOfBirth;
            employee.DailyRate = updateDto.DailyRate;
            employee.WorkingDays = updateDto.WorkingDays;

            await _repository.UpdateAsync(employee);
            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<decimal> ComputePayAsync(int employeeId)
        {
            // Call the stored procedure to compute pay
            var result = await _repository.ExecuteScalarAsync<decimal>(
                "EXEC ComputePayAsync @EmployeeId",
                new SqlParameter("@EmployeeId", employeeId));

            // If the stored procedure returns null or 0, check if employee exists
            // (the stored procedure returns 0 for non-existent employees)
            if (result == 0)
            {
                var employee = await _repository.GetByIdAsync(employeeId);
                if (employee == null)
                    throw new KeyNotFoundException("Employee not found");
            }

            return result;
        }

        private string GenerateEmployeeNumber(string lastName, DateTime dateOfBirth)
        {
            // Take first 3 letters of last name (uppercase)
            string prefix = lastName.Length >= 3
                ? lastName.Substring(0, 3).ToUpper()
                : lastName.PadRight(3, 'X').ToUpper();

            // Generate random 5-digit number
            Random random = new Random();
            int randomNumber = random.Next(10000, 99999); // 5 digits

            // Format date of birth as ddMMMyyyy (e.g., 15JAN1990)
            string datePart = dateOfBirth.ToString("ddMMMyyyy").ToUpper();

            return $"{prefix}-{randomNumber:D5}-{datePart}";
        }
    }
}