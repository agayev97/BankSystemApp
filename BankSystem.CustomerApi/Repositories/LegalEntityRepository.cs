using BankSystem.CustomerApi.Data;
using BankSystem.CustomerApi.Models;
using BankSystem.CustomerApi.Repositories.Interfaces;
using Dapper;
using System.Data;

namespace BankSystem.CustomerApi.Repositories
{
    public class LegalEntityRepository: ILegalEntityRepository
    {
        private readonly DbConnectionFactory _connectionFactory;
        public LegalEntityRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<LegalEntity?> GetByCustomerIdAsync(int customerId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<LegalEntity>(
                "usp_GetLegalEntityByCustomerId",
                new { CustomerId = customerId },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<int> CreateAsync(LegalEntity legalEntity)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(
                "usp_CreateLegalEntity",
                new
                {
                    legalEntity.CustomerId,
                    legalEntity.EntityName,
                    legalEntity.Voen,
                    legalEntity.EntityType,
                    legalEntity.PhoneNumber,
                    legalEntity.Email
                },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<bool> UpdateAsync(LegalEntity legalEntity)
        {
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.ExecuteAsync(
                "usp_UpdateLegalEntity",
                new
                {
                    legalEntity.CustomerId,
                    legalEntity.EntityName,
                    legalEntity.Voen,
                    legalEntity.EntityType,
                    legalEntity.PhoneNumber,
                    legalEntity.Email
                },
                commandType: CommandType.StoredProcedure);
            return result > 0;
        }
    }
}
