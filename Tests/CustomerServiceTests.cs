
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaPub.Tests
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options) 
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }

    [TestFixture]
    public class CustomerServiceTests
    {
         TestDbContext _context;
        private CustomerService _customerService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()                
                .Options;

            _context = new TestDbContext(options);
            _customerService = new CustomerService(new ProvaPub.Repository.TestDbContext(options));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void ListCustomers_returns_CorrectPageSize()
        {
            //Arrange
            int expectedPageSize = 10;
            int expectedPage = 1;

            //Act
            var customerList = _customerService.ListCustomers(expectedPage);

            //Assert
            Assert.AreEqual(expectedPageSize, customerList.Items.Count);
        }

        [Test]
        public void ListCustomers_Throws_Exception_For_NegativePageValue()
        {
            //Arrange
            int expectedPage = -1;

            //Act and Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _customerService.ListCustomers(expectedPage);
            });
        }

        [Test]
        public void CanPurchase_Returns_True_ForValid_Cutomer_And_PurchaseValue()
        {
            //Arrange
            int customerId = 1;
            decimal purchaseValue = 10;

            _context.Customers.AddRange(new[]
            {
                new Customer { Id = 1, Name = "Mario"},
                new Customer { Id = 2, Name = "Luigi"}
            });

            _context.Orders.AddRange(new[]
            {
                new Order { Id = 1, CustomerId = 1, OrderDate = DateTime.UtcNow.AddMonths(-1)}
            });

            _context.SaveChanges();

            //Act
            var canPuchase = _customerService.CanPurchase(customerId, purchaseValue);

            //Assert
            Assert.IsTrue(canPuchase.Result);
        }

        [Test]
        public void CanPurchase_Throws_ArgumentOutOfRangeException_For_Invalid_CustomerId()
        {
            //Arrange
            int customerId = -1;
            decimal purchaseValue = 10;

            //Act and Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                await _customerService.CanPurchase(customerId, purchaseValue);
            });
        }
        [Test]
        public void CanPurchase_Throws_ArgumentOutOfRangeException_For_Invalid_PurchaseValue()
        {
            // Arrange
            int customerId = 1;
            decimal purchaseValue = -10;

            // Act and Assert
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                await _customerService.CanPurchase(customerId, purchaseValue);
            });
        }

        [Test]
        public void CanPurchase_Throws_InvalidOperationException_If_Customer_Does_Not_Exist()
        {
            // Arrange
            int customerId = 1;
            decimal purchaseValue = 10;

            // Act and Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _customerService.CanPurchase(customerId, purchaseValue);
            });
        }

        [Test]
        public void CanPurchase_Returns_False_If_Customer_Made_Purchase_This_Month()
        {
            // Arrange
            int customerId = 1;
            decimal purchaseValue = 10;

            _context.Customers.AddRange(new[]
            {
                new Customer { Id = 1, Name = "Bowser" }
            });
            _context.Orders.AddRange(new[]
            {
                new Order { Id = 1, CustomerId = 1, OrderDate = DateTime.UtcNow }
            });
            _context.SaveChanges();

            // Act
            var canPurchase = _customerService.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.IsFalse(canPurchase.Result);
        }

        [Test]
        public void CanPurchase_Returns_False_If_Customer_Never_Bought_Before_And_PurchaseValue_Is_Too_High()
        {
            // Arrange
            int customerId = 1;
            decimal purchaseValue = 200;

            _context.Customers.AddRange(new[]
            {
                new Customer { Id = 1, Name = "Toad" }
            });
            _context.SaveChanges();

            // Act
            var canPurchase = _customerService.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.IsFalse(canPurchase.Result);
        }

        [Test]
        public void CanPurchase_Returns_True_If_Customer_Never_Bought_Before_And_PurchaseValue_Is_Valid()
        {
            // Arrange
            int customerId = 1;
            decimal purchaseValue = 50;

            _context.Customers.AddRange(new[]
            {
                new Customer { Id = 1, Name = "Peach" }
            });
            _context.SaveChanges();

            // Act
            var canPurchase = _customerService.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.IsTrue(canPurchase.Result);
        }
    }
}
