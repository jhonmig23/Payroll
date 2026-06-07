using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Api.Data;
using Microsoft.Data.SqlClient;

namespace Api.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await GetByIdAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, params object[] parameters)
        {
            // Convert parameters to proper format for EF Core
            var formattedParameters = FormatParameters(parameters);

            // For scalar types (value types and string), we use SqlQueryRaw with AsAsyncEnumerable to avoid composability issues
            if (typeof(T).IsValueType || typeof(T) == typeof(string))
            {
                // Use SqlQueryRaw with AsAsyncEnumerable for scalar types to prevent composability issues
                var result = await _context.Database
                    .SqlQueryRaw<ScalarResult<T>>(sql, formattedParameters)
                    .AsAsyncEnumerable()
                    .FirstOrDefaultAsync();

                if (result == null)
                    return default;

                if (result.Value == null || Convert.IsDBNull(result.Value))
                    return default;

                // Add explicit conversion to prevent casting issues
                return (T)Convert.ChangeType(result.Value, typeof(T));
            }
            else
            {
                // For reference types (classes), use SqlQueryRaw directly
                var result = await _context.Database
                    .SqlQueryRaw<T>(sql, formattedParameters)
                    .FirstOrDefaultAsync();
                return result;
            }
        }

        private object[] FormatParameters(object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                return parameters;

            // Check if we have a single SqlParameter or a mix of parameters
            var hasSqlParameter = parameters.Any(p => p is SqlParameter);

            // If we have SqlParameters, we need to handle them specially
            if (hasSqlParameter)
            {
                // For now, just return as-is - EF Core should handle SqlParameter objects correctly
                return parameters;
            }

            // Check if we have a single anonymous object that should be expanded
            if (parameters.Length == 1 && parameters[0] != null &&
                !parameters[0].GetType().IsPrimitive &&
                !parameters[0].GetType().IsEnum &&
                parameters[0].GetType() != typeof(string))
            {
                // This is likely an anonymous object - return it as-is for EF Core to expand
                return parameters;
            }

            return parameters;
        }

        // Wrapper class for scalar results
        private class ScalarResult<TValue>
        {
            public TValue Value { get; set; }
        }
    }
}