using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using CustomerDetailsService.Controllers;
using CustomerDetailsService.Models;
using CustomerDetailsService.Models.Data;
using CustomerDetailsService.Service;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace CustomerDetails.Test
{
	public class CustomerDetailsTest
	{
		private readonly Mock<ICustomerService> _customerService;
		private readonly IFixture _fixture;
		private readonly CustomerDetailsController _customerDetailsController;
		public CustomerDetailsTest()
		{
			_fixture = new Fixture();
			_customerService = _fixture.Freeze<Mock<ICustomerService>>();
			_customerDetailsController = new CustomerDetailsController(_customerService.Object, new NullLogger<CustomerDetailsController>());
		}
		[Fact]
		public async Task GetAllCustomer_SuccessTest()
		{
			//arrange
			var customerList = new CustomersModel
			{
				Customers = GetCustomersData().ToArray()
			};
			_customerService.Setup(x => x.GetAllCustomers())
			.ReturnsAsync(customerList);

			//act
			var customerResults = await _customerDetailsController.GetAllCustomers();

			//assert
			customerResults.Should().NotBeNull();
			customerResults.Customers.Length.Should().Be(GetCustomersData().Count);
			customerResults.Customers.Should().BeEquivalentTo(customerList.Customers);
		}

		[Fact]
		public async Task GetCustomerByID_SuccessTest()
		{
			//arrange
			var customerList = GetCustomersData();
			_customerService.Setup(x => x.GetCustomerById(2))
		   .ReturnsAsync(customerList[1]);
			//act
			var customerResult = await _customerDetailsController.GetCustomerById(2);

			//assert
			customerResult.Should().NotBeNull();
			customerResult.Id.Should().Be(customerList[1].Id);
		}

		[Fact]
		public async Task AddCustomer_SuccessTest()
		{
			var addressModel = new AddressModel
			{
				StreetName = RandomStringGenerator.RandomString(8),
				City = RandomStringGenerator.RandomString(8),
				HouseNumber = new Random().Next(1, 20).ToString(),
				Country = "Netherlands",
				PostalCode = new Random().Next(3000, 3500).ToString() + RandomStringGenerator.RandomString(2).ToString().ToUpper()
			};
			var customerMock = new AddCustomerRequestModel(
 			  RandomStringGenerator.RandomString(8),
				RandomStringGenerator.RandomString(9),
				$"{RandomStringGenerator.RandomString(5)}@example.com",
					new Random().Next(18, 50),
					addressModel);

			var response = _fixture.Create<bool>();
			_customerService.Setup(x => x.AddCustomer(customerMock)).ReturnsAsync(true);
			var result = await _customerDetailsController.AddNewCustomer(customerMock).ConfigureAwait(false);
			//assert
			result.Should().NotBeNull();

		}


 		private List<CustomerModel> GetCustomersData()
		{
			List<CustomerModel> customersData = new List<CustomerModel>
			{
				new CustomerModel()
				{
					Id =1,
					FirstName = RandomStringGenerator.RandomString(8),
					LastName = RandomStringGenerator.RandomString(9),
					Age = new Random().Next(18,50),
					Email = $"{RandomStringGenerator.RandomString(5)}@example.com",
					CurrentAddress = new AddressModel
					{
						StreetName = RandomStringGenerator.RandomString(8),
						City = RandomStringGenerator.RandomString(8),
						HouseNumber = new Random().Next(1,20).ToString(),
						Country = "Netherlands",
 						PostalCode = new Random().Next(3000,3500).ToString()+RandomStringGenerator.RandomString(2).ToString().ToUpper()
					}
				},
				new CustomerModel()
				{
					Id =2,
					FirstName = RandomStringGenerator.RandomString(8),
					LastName = RandomStringGenerator.RandomString(9),
					Email = $"{RandomStringGenerator.RandomString(5)}@example.com",
					Age = new Random().Next(18,50),
					CurrentAddress = new AddressModel
					{
						StreetName = RandomStringGenerator.RandomString(8),
						City = RandomStringGenerator.RandomString(8),
						HouseNumber = new Random().Next(1,20).ToString(),
						Country = "Netherlands",
						PostalCode = new Random().Next(3000,3500).ToString()+RandomStringGenerator.RandomString(2).ToString().ToUpper()
					}
				},
				new CustomerModel()
				{
					Id =3,
					FirstName = RandomStringGenerator.RandomString(8),
					LastName = RandomStringGenerator.RandomString(9),
					Email = $"{RandomStringGenerator.RandomString(5)}@example.com",
					Age = new Random().Next(18,50),
					CurrentAddress = new AddressModel
					{
						StreetName = RandomStringGenerator.RandomString(8),
						City = RandomStringGenerator.RandomString(8),
						HouseNumber = new Random().Next(1,20).ToString(),
						Country = "Netherlands",
						PostalCode = new Random().Next(3000,3500).ToString()+RandomStringGenerator.RandomString(2).ToString().ToUpper()
					}
				},
			};
			return customersData;
		}
	}
}