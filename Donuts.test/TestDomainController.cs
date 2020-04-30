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

namespace DonutsTest
{
    public class TestDomainController
    {
        //private readonly HttpClient _client;

        public TestDomainController()
        {
            //var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            //_client = server.CreateClient()
        }


        [Fact]
        public async Task GetAllDomain()
        {

            //int testSessionId = 123;
            var mockDomainRepo = new Mock<IDomainRepository>();
            var mockUserRepo = new Mock<IUserRepository>();
            mockDomainRepo.Setup(repo => repo.GetAllDomains())
                .Returns(CreateDomainTestData());
            mockUserRepo.Setup(repo => repo.GetAllUsers())
                .Returns(CreateUserTestData());

            var controller = new DomainsController(mockDomainRepo.Object, mockUserRepo.Object);
            var result = await controller.GetAllDomain() as OkObjectResult;
            //var list = result.Value;
            var test = result.Value as IEnumerable<Domain>;
            var list = test.ToList();
            Assert.Equal(2, list.Count);
            Assert.NotNull(result);

        }


        [Fact]
        public async Task GetDomain()
        {
            var mockDomainRepo = new Mock<IDomainRepository>();
            var mockUserRepo = new Mock<IUserRepository>();

            mockDomainRepo.Setup(repo => repo.GetAllDomains())
                .Returns(CreateDomainTestData());
            mockUserRepo.Setup(repo => repo.GetAllUsers())
                .Returns(CreateUserTestData());

            var controller = new DomainsController(mockDomainRepo.Object, mockUserRepo.Object);
            var result = await controller.GetDomain(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var res = okResult.Value;
            //var res = Assert.IsType<IActionResult>(okResult.Value);
            var idea = res as Domain;
            Assert.Equal(1, idea.DomainId);
            //Assert.Equal(1, notFoundObjectResult.Value);

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
                User = new User()
                {
                    UserId = 1,
                    UserName = "user1"
                }
            });
            session.Add(new Domain()
            {
                DomainId = 2,
                ExperiationDate = DateTime.Today.AddYears(1),
                Name = "123456789.software",
                RegistrationDate = DateTime.Today,
                UserId = 1,
                User = new User()
                {
                    UserId = 1,
                    UserName = "user1"
                }
            });

            return session;
        }

        private List<User> CreateUserTestData()
        {
            var session = new List<User>();
            session.Add(new User()
            {
                UserId = 1,
                UserName = "user1",
                Domains = CreateDomainTestData().ToList()
            });
            session = new List<User>();
            session.Add(new User()
            {
                UserId = 2,
                UserName = "user2"
            });
            return session;
        }
    }
}