using EmployeeManagement.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EmployeeManagement.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<Employee> _employees;

        public MongoDbService(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _employees = database.GetCollection<Employee>("Employees");
        }

        public async Task<List<Employee>> GetAsync() => await _employees.Find(employee => true).ToListAsync();

        public async Task<Employee> GetAsync(string id) => await _employees.Find(employee => employee.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Employee employee) => await _employees.InsertOneAsync(employee);

        public async Task UpdateAsync(string id, Employee employeeIn) => await _employees.ReplaceOneAsync(employee => employee.Id == id, employeeIn);

        public async Task RemoveAsync(string id) => await _employees.DeleteOneAsync(employee => employee.Id == id);
    }
}

