using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<EmployeeViewModel>> GetEmployeesAsync()
        {
            var response = await _httpClient.GetAsync("api/employees");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<EmployeeViewModel>>();
            if (result == null)
            {
                throw new InvalidOperationException("API returned successful response but failed to deserialize employee list.");
            }
            return result;
        }

        public async Task<EmployeeViewModel> GetEmployeeAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/employees/{id}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<EmployeeViewModel>();
            if (result == null)
            {
                throw new InvalidOperationException($"API returned successful response but failed to deserialize employee with ID {id}.");
            }
            return result;
        }

        public async Task<EmployeeViewModel> CreateEmployeeAsync(EmployeeViewModel employee)
        {
            var response = await _httpClient.PostAsJsonAsync("api/employees", employee);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<EmployeeViewModel>();
            if (result == null)
            {
                throw new InvalidOperationException("API returned successful response but failed to deserialize employee data.");
            }

            return result;
        }

        public async Task UpdateEmployeeAsync(EmployeeViewModel employee)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/employees/{employee.Id}", employee);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/employees/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<decimal> ComputePayAsync(int employeeId)
        {
            var response = await _httpClient.GetAsync($"api/employees/compute-pay?employeeId={employeeId}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<decimal>();
            if (result == null)
            {
                throw new InvalidOperationException("API returned successful response but failed to deserialize pay amount.");
            }
            return result;
        }
    }
}