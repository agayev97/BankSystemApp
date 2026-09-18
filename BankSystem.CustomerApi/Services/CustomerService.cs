using System.Transactions;
using BankSystem.CustomerApi.DTOs.Customer;
using BankSystem.CustomerApi.DTOs.Individual;
using BankSystem.CustomerApi.DTOs.LegalEntity;
using BankSystem.CustomerApi.Models;
using BankSystem.CustomerApi.Repositories.Interfaces;
using BankSystem.CustomerApi.Services.Interfaces;

namespace BankSystem.CustomerApi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IIndividualRepository _individualRepository;
        private readonly ILegalEntityRepository _legalEntityRepository;

        public CustomerService(
            ICustomerRepository customerRepository,
            IIndividualRepository individualRepository,
            ILegalEntityRepository legalEntityRepository)
        {
            _customerRepository = customerRepository;
            _individualRepository = individualRepository;
            _legalEntityRepository = legalEntityRepository;
        }

        // =========================================================
        // CREATE INDIVIDUAL CUSTOMER
        // =========================================================

        public async Task<int> CreateIndividualCustomerAsync(
            CreateIndividualDto dto)
        {
            using var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);

            // 1. Customer yaradılır
            var customer = new Customer
            {
                CustomerType = "Individual",
                IsActive = true
            };

            int customerId =
                await _customerRepository.CreateAsync(customer);

            // 2. Individual məlumatları yaradılır
            var individual = new Individual
            {
                CustomerId = customerId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                FatherName = dto.FatherName,
                IdentityNo = dto.IdentityNo,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email
            };

            await _individualRepository.CreateAsync(individual);

            // Hər iki əməliyyat uğurludursa commit
            scope.Complete();

            return customerId;
        }


        // =========================================================
        // CREATE LEGAL ENTITY CUSTOMER
        // =========================================================

        public async Task<int> CreateLegalEntityCustomerAsync(
            CreateLegalEntityDto dto)
        {
            using var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);

            // 1. Customer yaradılır
            var customer = new Customer
            {
                CustomerType = "LegalEntity",
                IsActive = true
            };

            int customerId =
                await _customerRepository.CreateAsync(customer);

            // 2. LegalEntity məlumatları yaradılır
            var legalEntity = new LegalEntity
            {
                CustomerId = customerId,
                EntityName = dto.EntityName,
                Voen = dto.Voen,
                EntityType = dto.EntityType,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email
            };

            await _legalEntityRepository.CreateAsync(legalEntity);

            // Hər iki əməliyyat uğurludursa commit
            scope.Complete();

            return customerId;
        }


        // =========================================================
        // GET CUSTOMER BY ID
        // =========================================================

        public async Task<CustomerDetailsDto?> GetCustomerByIdAsync(int id)
        {
            // Əvvəl əsas Customer tapılır
            var customer =
                await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return null;

            var result = new CustomerDetailsDto
            {
                CustomerId = customer.Id,
                CustomerType = customer.CustomerType,
                CreatedDate = customer.CreatedDate,
                IsActive = customer.IsActive
            };

            // =====================================================
            // INDIVIDUAL
            // =====================================================

            if (customer.CustomerType == "Individual")
            {
                var individual =
                    await _individualRepository.GetByCustomerIdAsync(id);

                if (individual != null)
                {
                    result.Individual = new IndividualResponseDto
                    {
                        CustomerId = individual.CustomerId,
                        FirstName = individual.FirstName,
                        LastName = individual.LastName,
                        FatherName = individual.FatherName,
                        IdentityNo = individual.IdentityNo,
                        PhoneNumber = individual.PhoneNumber,
                        Email = individual.Email
                    };
                }
            }

            // =====================================================
            // LEGAL ENTITY
            // =====================================================

            else if (customer.CustomerType == "LegalEntity")
            {
                var legalEntity =
                    await _legalEntityRepository.GetByCustomerIdAsync(id);

                if (legalEntity != null)
                {
                    result.LegalEntity = new LegalEntityResponseDto
                    {
                        CustomerId = legalEntity.CustomerId,
                        EntityName = legalEntity.EntityName,
                        Voen = legalEntity.Voen,
                        EntityType = legalEntity.EntityType,
                        PhoneNumber = legalEntity.PhoneNumber,
                        Email = legalEntity.Email
                    };
                }
            }

            return result;
        }


        // =========================================================
        // GET ALL CUSTOMERS
        // =========================================================

        public async Task<IEnumerable<CustomerDetailsDto>>
            GetAllCustomersAsync()
        {
            var customers =
                await _customerRepository.GetAllAsync();

            var customerList =
                new List<CustomerDetailsDto>();

            foreach (var customer in customers)
            {
                var customerDetails =
                    await GetCustomerByIdAsync(customer.Id);

                if (customerDetails != null)
                {
                    customerList.Add(customerDetails);
                }
            }

            return customerList;
        }


        // =========================================================
        // DEACTIVATE CUSTOMER
        // =========================================================

        public async Task<bool> DeactivateCustomerAsync(int id)
        {
            // Customer-i fiziki silmirik.
            // IsActive = 0 edirik.

            return await _customerRepository.DeleteAsync(id);
        }
    }
}