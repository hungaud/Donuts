using Donuts.Controllers;
using Donuts.Models;
using Donuts.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Donuts.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DonutsTest
{
    public class TestDomainController
    {

        public TestDomainController()
        {
        }


        [Fact]
        public async Task GetAllDomain()
        {
            // Arrange
            var mockDomainRepo = new Mock<IDomainRepository>();
            var mockUserRepo = new Mock<IUserRepository>();
            mockDomainRepo.Setup(repo => repo.GetAllDomains())
                .Returns(CreateDomainTestData());
            mockUserRepo.Setup(repo => repo.GetAllCustomers())
                .Returns(CreateCustomerData());

            // Act
            var controller = new DomainsController(mockDomainRepo.Object, mockUserRepo.Object);
            var result = await controller.GetAllDomain() as OkObjectResult;
            var test = result.Value as IEnumerable<Domain>;
            var list = test.ToList();

            // Assert
            Assert.Equal(2, list.Count);
            Assert.NotNull(result);

        }


        [Fact]
        public async Task TestGetDomainById()
        {
            // Arrange
            var mockDomainRepo = new Mock<IDomainRepository>();
            var mockUserRepo = new Mock<IUserRepository>();

            mockDomainRepo.Setup(repo => repo.GetDomain(1))
                .ReturnsAsync(CreateDomainTestData()[0]);

            // Act
            var controller = new DomainsController(mockDomainRepo.Object, mockUserRepo.Object);
            var result = await controller.GetDomain(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var res = Assert.IsType<Domain>(okResult.Value);
            var idea = res as Domain;
            Assert.Equal(1, idea.DomainId);
            Assert.Equal("abcdefghi.software", idea.Name);
            Assert.Equal(DateTime.Today.AddYears(1), idea.ExperiationDate);
        }

        [Fact]
        public async Task TestPostDomain()
        {
            // Arrange
            var dto = new Domain()
            {
                DomainId = 1,
                Name = "abcdefghi.software",
                RegistrationDate = DateTime.Today,
                UserId = 1,
                User = new Customer()
                {
                    CustomerId = 1,
                    CustomerName = "user1"
                }
            };

            var experiationDuration = TimeDuration.YEAR;
            int experiationLength = 1;
            DateTime today = DateTime.Today;

            dto.ExperiationDate = experiationDuration == TimeDuration.YEAR ? today.AddYears(experiationLength) : today.AddMonths(experiationLength);

            var mockDomainRepo = new Mock<IDomainRepository>();
            var mockUserRepo = new Mock<IUserRepository>();
            mockDomainRepo.Setup(repo => repo.AddDomain(dto))
                .ReturnsAsync(dto);
            mockUserRepo.Setup(repo => repo.GetCustomer(1))
                .ReturnsAsync(CreateCustomerData()[0]);


            // Act
            var controller = new DomainsController(mockDomainRepo.Object, mockUserRepo.Object);
            SimulateValidation(dto, controller);
            var result = await controller.PostDomain(dto);

            // Assert
            var okResult = Assert.IsType<CreatedAtActionResult>(result);
            var res = Assert.IsType<Domain>(okResult.Value);
            var idea = res as Domain;
            Assert.Equal(1, idea.DomainId);
            Assert.Equal("abcdefghi.software", idea.Name);
            Assert.Equal(DateTime.Today.AddYears(1), idea.ExperiationDate);
        }

        // Handles Data Annotation validation
        private void SimulateValidation(object model, DomainsController controller)
        {
            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }
        }

        private List<Domain> CreateDomainTestData()
        {
            var session = new List<Domain>();
            session.Add(new Domain()
            {
                DomainId = 1,
                ExperiationDate = DateTime.Today.AddYears(1),
                Name = "abcdefghi.software",
                RegistrationDate = DateTime.Today,
                UserId = 1,
                User = new Customer()
                {
                    CustomerId = 1,
                    CustomerName = "user1"
                }
            });
            session.Add(new Domain()
            {
                DomainId = 2,
                ExperiationDate = DateTime.Today.AddYears(1),
                Name = "123456789.software",
                RegistrationDate = DateTime.Today,
                UserId = 1,
                User = new Customer()
                {
                    CustomerId = 1,
                    CustomerName = "user1"
                }
            });

            return session;
        }

        private List<Customer> CreateCustomerData()
        {
            var session = new List<Customer>();
            session.Add(new Customer()
            {
                CustomerId = 1,
                CustomerName = "user1",
                Domains = CreateDomainTestData().ToList()
            });
            session.Add(new Customer()
            {
                CustomerId = 2,
                CustomerName = "user2"
            });
            return session;
        }
    }
}