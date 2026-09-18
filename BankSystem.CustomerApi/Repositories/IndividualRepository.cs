using BankSystem.CustomerApi.Data;
using BankSystem.CustomerApi.Models;
using BankSystem.CustomerApi.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace BankSystem.CustomerApi.Repositories
{
    public class IndividualRepository : IIndividualRepository
    {
        private readonly DbConnectionFactory _connectionFactory;
        public IndividualRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Individual?> GetByCustomerIdAsync(int customerId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Individual>(
                "usp_GetIndividualByCustomerId",
                new { CustomerId = customerId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> CreateAsync(Individual individual)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(
                "usp_CreateIndividual",
                new
                {
                    individual.CustomerId,
                    individual.FirstName,
                    individual.LastName,
                    individual.FatherName,
                    individual.IdentityNo,
                    individual.PhoneNumber,
                    individual.Email
                },
                commandType: CommandType.StoredProcedure);
           
        }

        public async Task<bool> UpdateAsync(Individual individual)
        {
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.ExecuteAsync(
                "usp_UpdateIndividual",
                new
                {
                    individual.CustomerId,
                    individual.FirstName,
                    individual.LastName,
                    individual.FatherName,
                    individual.IdentityNo,
                    individual.PhoneNumber,
                    individual.Email
                },
                commandType: CommandType.StoredProcedure);
            return result > 0;
        }   
    }
}
