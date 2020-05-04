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
using Microsoft.AspNetCore.Http;

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
            var mockCustomerRepo = new Mock<ICustomerRepository>();
            mockDomainRepo.Setup(repo => repo.GetAllDomains())
                .Returns(CreateDomainTestData());
            mockCustomerRepo.Setup(repo => repo.GetAllCustomers())
                .Returns(CreateCustomerData());

            // Act
            var controller = new DomainsController(mockDomainRepo.Object, mockCustomerRepo.Object);
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
            var mockCustomerRepo = new Mock<ICustomerRepository>();

            mockDomainRepo.Setup(repo => repo.GetDomain(1))
                .ReturnsAsync(CreateDomainTestData()[0]);

            // Act
            var controller = new DomainsController(mockDomainRepo.Object, mockCustomerRepo.Object);
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
        public async Task TestGetDomainByName()
        {
            // Arrange
            var mockDomainRepo = new Mock<IDomainRepository>();
            var mockCustomerRepo = new Mock<ICustomerRepository>();

            mockDomainRepo.Setup(repo => repo.GetDomain("abcdefghi.software"))
                .ReturnsAsync(CreateDomainTestData()[0]);

            // Act
            var controller = new DomainsController(mockDomainRepo.Object, mockCustomerRepo.Object);
            var result = await controller.GetDomain("abcdefghi.software");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var res = Assert.IsType<Domain>(okResult.Value);
            var idea = res as Domain;
            Assert.Equal(1, idea.DomainId);
            Assert.Equal("abcdefghi.software", idea.Name);
            Assert.Equal(DateTime.Today.AddYears(1), idea.ExperiationDate);
        }

        [Fact]
        public async Task TestPostDomainsValidDomainName()
        {
            // Arrange
            var domainDTO = CreateDomainTestData()[0];
            var customerDTO = CreateCustomerData()[0];

            var mockDomainRepo = new Mock<IDomainRepository>();
            var mockCustomerRepo = new Mock<ICustomerRepository>();
            mockDomainRepo.Setup(repo => repo.AddDomain(domainDTO))
                .ReturnsAsync(domainDTO);
            mockCustomerRepo.Setup(repo => repo.GetCustomer(1))
                .ReturnsAsync(customerDTO);

            // Act
            var controller = new DomainsController(mockDomainRepo.Object, mockCustomerRepo.Object);
            SimulateModelValidation(domainDTO, controller);
            var result = await controller.PostDomain(domainDTO, TimeDuration.YEAR, 1);

            // Assert
            var okResult = Assert.IsType<CreatedAtActionResult>(result);
            var res = Assert.IsType<Domain>(okResult.Value);
            var idea = res as Domain;
            Assert.Equal(1, idea.DomainId);
            Assert.Equal("abcdefghi.software", idea.Name);
            Assert.Equal(DateTime.Today.AddYears(1), idea.ExperiationDate);
        }

        // Should fail due to Domain name is < 10 characters
        [Fact]
        public async Task TestPostInvalidDomainName()
        {
            // Arrange
            var domainDTO = CreateDomainTestData()[1];
            var customerDTO = CreateCustomerData()[0];


            var mockDomainRepo = new Mock<IDomainRepository>();
            var mockCustomerRepo = new Mock<ICustomerRepository>();
            mockDomainRepo.Setup(repo => repo.AddDomain(domainDTO))
                .ReturnsAsync(domainDTO);
            mockCustomerRepo.Setup(repo => repo.GetCustomer(1))
                .ReturnsAsync(customerDTO);

            // Act
            var controller = new DomainsController(mockDomainRepo.Object, mockCustomerRepo.Object);
            SimulateModelValidation(domainDTO, controller);
            var result = await controller.PostDomain(domainDTO, TimeDuration.YEAR, 1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task TestDeleteDomain()
        {
            var domainDTO = CreateDomainTestData()[0];
            var customerDTO = CreateCustomerData()[0];

            var mockDomainRepo = new Mock<IDomainRepository>();
            var mockCustomerRepo = new Mock<ICustomerRepository>();

            mockDomainRepo.Setup(repo => repo.DeleteDomain(domainDTO.DomainId))
                .ReturnsAsync(domainDTO);

            // Act
            var controller = new DomainsController(mockDomainRepo.Object, mockCustomerRepo.Object);
            var result = await controller.GetDomain(domainDTO.DomainId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var res = Assert.IsType<Domain>(okResult.Value);
            var idea = res as Domain;
            Assert.Equal(1, idea.DomainId);
            Assert.Equal("abcdefghi.software", idea.Name);
            Assert.Equal(DateTime.Today.AddYears(1), idea.ExperiationDate);
        }

        // Handles Data Annotation validation
        private void SimulateModelValidation(object model, DomainsController controller)
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
            //valid name
            session.Add(new Domain()
            {
                DomainId = 1,
                Name = "abcdefghi.software",
                RegistrationDate = DateTime.Today,
                CustomerId = 1,
                Customer = new Customer()
                {
                    CustomerId = 1,
                    CustomerName = "user1"
                }
            });

            // invalid name
            session.Add(new Domain()
            {
                DomainId = 2,
                ExperiationDate = DateTime.Today.AddYears(1),
                Name = "abc",
                RegistrationDate = DateTime.Today,
                CustomerId = 1,
                Customer = new Customer()
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