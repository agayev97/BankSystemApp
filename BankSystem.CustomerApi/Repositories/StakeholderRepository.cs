using BankSystem.CustomerApi.Data;
using BankSystem.CustomerApi.Models;
using BankSystem.CustomerApi.Repositories.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;

namespace BankSystem.CustomerApi.Repositories
{
    public class StakeholderRepository : IStakeholderRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public StakeholderRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Stakeholder>> GetByLegalEntityIdAsync(int legalEntityId)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Stakeholder>(
                "usp_GetStakeholdersByLegalEntityId",
                new { LegalEntityId = legalEntityId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> CreateAsync(Stakeholder stakeholder)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(
            "usp_CreateStakeholder",
             new
             {
                 stakeholder.LegalEntityId,
                 stakeholder.PartyType,
                 stakeholder.RelationType,
                 stakeholder.FullName,
                 stakeholder.IdentityNumber,
                 stakeholder.Voen,
                 stakeholder.PhoneNumber,
                 stakeholder.Email
             },
               commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> UpdateAsync(Stakeholder stakeholder)
        {
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(
            "usp_UpdateStakeholder",
            new
            {
            stakeholder.Id,
            stakeholder.LegalEntityId,
            stakeholder.PartyType,
            stakeholder.RelationType,
            stakeholder.FullName,
            stakeholder.IdentityNumber,
            stakeholder.Voen,
            stakeholder.PhoneNumber,
            stakeholder.Email
            },
            commandType: CommandType.StoredProcedure);

            return result == 1;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(
                "usp_DeactivateStakeholder",
                new { StakeholderId = id },
                commandType: CommandType.StoredProcedure);
            return result == 1;
        }

    }
}
