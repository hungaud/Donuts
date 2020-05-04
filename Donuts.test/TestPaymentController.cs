using Donuts.Controllers;
using Donuts.Models;
using Donuts.Models.Enums;
using Donuts.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Donuts.test
{
    public class TestPaymentController
    {

        public TestPaymentController() { }

        [Fact]
        public async Task TestGetPaymentFromCustomer()
        {
            // Arrange
            var mockPaymentRepo = new Mock<IPaymentRepository>();
            var mockCustomerRepo = new Mock<ICustomerRepository>();
            var paymentDTO = CreatePaymentData();
            mockPaymentRepo.Setup(repo => repo.GetPaymentFromCustomer(paymentDTO.CustomerId))
                .ReturnsAsync(paymentDTO);

            // Act
            var controller = new PaymentsController(mockPaymentRepo.Object, mockCustomerRepo.Object);
            var result = await controller.GetPaymentFromCustomer(paymentDTO.CustomerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var res = Assert.IsType<Payment>(okResult.Value);
            var idea = res as Payment;
            Assert.Equal(1, idea.PaymentId);
        }

        [Fact]
        public async Task TestPostPaymentValid()
        {
            // Arrange
            var mockPaymentRepo = new Mock<IPaymentRepository>();
            var mockCustomerRepo = new Mock<ICustomerRepository>();
            var paymentDTO = CreatePaymentData();

            mockPaymentRepo.Setup(repo => repo.GetPaymentFromCustomer(paymentDTO.CustomerId))
                .ReturnsAsync(paymentDTO);
            mockCustomerRepo.Setup(repo => repo.GetCustomer(paymentDTO.CustomerId))
                .ReturnsAsync(paymentDTO.Customer);

            // Act
            var controller = new PaymentsController(mockPaymentRepo.Object, mockCustomerRepo.Object);
            SimulateModelValidation.Validate(mockPaymentRepo, controller);
            var result = await controller.PostPayment(paymentDTO);

            // Assert
            var okResult = Assert.IsType<CreatedAtActionResult>(result);
            var res = Assert.IsType<Payment>(okResult.Value);
            var idea = res as Payment;
            Assert.Equal(1, idea.PaymentId);
            Assert.NotNull(idea.Customer);
            Assert.NotNull(idea.Domain);
            Assert.Equal(GetLastDayOfMonth(), idea.DueDate);
            Assert.Equal(PaymentStatus.CREATED, idea.Status);
        }

        [Fact]
        public async Task TestPostPaymentNoAmount()
        {
            // Arrange
            var mockPaymentRepo = new Mock<IPaymentRepository>();
            var mockCustomerRepo = new Mock<ICustomerRepository>();
            var paymentDTO = CreatePaymentData();
            paymentDTO.Amount = 0;
            mockPaymentRepo.Setup(repo => repo.GetPaymentFromCustomer(paymentDTO.CustomerId))
                .ReturnsAsync(paymentDTO);
            mockCustomerRepo.Setup(repo => repo.GetCustomer(paymentDTO.CustomerId))
                .ReturnsAsync(paymentDTO.Customer);

            // Act
            var controller = new PaymentsController(mockPaymentRepo.Object, mockCustomerRepo.Object);
            SimulateModelValidation.Validate(mockPaymentRepo, controller);
            var result = await controller.PostPayment(paymentDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Theory]
        [InlineData(50)]
        [InlineData(100)]
        public async Task TestPutPaymentAmount(decimal amountPaid)
        {
            // Arrange
            var mockPaymentRepo = new Mock<IPaymentRepository>();
            var mockCustomerRepo = new Mock<ICustomerRepository>();
            var paymentDTO = CreatePaymentData();
            paymentDTO.AmountPaid = amountPaid;

            mockPaymentRepo.Setup(repo => repo.GetPaymentFromCustomer(paymentDTO.CustomerId))
                .ReturnsAsync(paymentDTO);
            mockCustomerRepo.Setup(repo => repo.GetCustomer(paymentDTO.CustomerId))
                .ReturnsAsync(paymentDTO.Customer);

            // Act
            var controller = new PaymentsController(mockPaymentRepo.Object, mockCustomerRepo.Object);
            SimulateModelValidation.Validate(mockPaymentRepo, controller);
            var result = await controller.PutPayment(paymentDTO.PaymentId, paymentDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var res = Assert.IsType<Payment>(okResult.Value);
            var idea = res as Payment;
            Assert.Equal(1, idea.PaymentId);
            Assert.NotNull(idea.Customer);
            Assert.NotNull(idea.Domain);
            if(paymentDTO.Amount == 0)
            {
                Assert.Equal(PaymentStatus.COMPLETE, idea.Status);
                Assert.Null(idea.DueDate);
            }
            else
            {
                Assert.NotNull(idea.DueDate);
                Assert.Equal(PaymentStatus.PROCESSING, idea.Status);
            }
        }


        private Payment CreatePaymentData()
        {
            return new Payment
            {
                Amount = 100,
                AmountPaid = 0,
                DueDate = GetLastDayOfMonth(),
                Customer = new Customer()
                {
                    CustomerId = 1,
                    CustomerName = "user1",
                    CardNumber = "0000-0000-0000-0000",
                },
                CustomerId = 1,
                Domain = new Domain()
                {
                    DomainId = 1,
                    ExperiationDate = DateTime.Today.AddYears(1),
                    Name = "abcdefghi.software",
                    RegistrationDate = DateTime.Today,
                    CustomerId = 1,
                    Customer = new Customer()
                    {
                        CustomerId = 1,
                        CustomerName = "user1"
                    }
                },
                DomainId = 1,
                PaymentId = 1,
                Status = PaymentStatus.CREATED
            };
        }

        // Doesn't account if last day of the month is today.
        private DateTime GetLastDayOfMonth()
        {
            DateTime date = DateTime.Today;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            return firstDayOfMonth.AddMonths(1).AddDays(-1);
        }
    }
}
