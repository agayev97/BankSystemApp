using BankSystem.CustomerApi.Data;
using Dapper;
using BankSystem.CustomerApi.Models;
using BankSystem.CustomerApi.Repositories.Interfaces;
using System.Data;

namespace BankSystem.CustomerApi.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public CustomerRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

       
        public async Task<int> CreateAsync(Customer customer)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(
                "usp_CreateCustomer",
                new { customer.CustomerType },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Customer>(
                "usp_GetCustomers", commandType: CommandType.StoredProcedure);
        }

        
        public async Task<Customer?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Customer>(
                "usp_GetCustomerById",
                new { CustomerId = id },
                commandType: CommandType.StoredProcedure);
        }

       
        public async Task<bool> UpdateAsync(Customer customer)
        {
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.ExecuteAsync(
                "usp_UpdateCustomer",
                new { CustomerId = customer.Id, customer.IsActive },   
                commandType: CommandType.StoredProcedure);
            return result > 0;
        }

       
        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.ExecuteAsync(
                "usp_DeactiveCustomer",
                new { CustomerId = id },  
                commandType: CommandType.StoredProcedure);
            return result > 0;
        }
    }
}