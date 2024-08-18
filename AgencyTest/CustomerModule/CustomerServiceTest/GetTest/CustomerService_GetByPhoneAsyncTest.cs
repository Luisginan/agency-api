using AgencyApi.CustomerModule.Models;
using AgencyApi.CustomerModule.Repository;
using AgencyApi.CustomerModule.Services;
using Moq;
using AgencyApi.CustomerModule.Validators;

namespace AgencyTest.CustomerServiceTest.GetTest;

public class CustomerServiceGetByPhoneAsyncTest
{
    [Fact]
    public async Task GetCustomerByNameAndPhoneAsync_NonNull()
    {
        var customerDal = new Mock<ICustomerRepository>();
        var customerRule = new Mock<ICustomerRuler>();
        customerDal.Setup(x => x.GetCustomerByNameAndPhone(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new Customer
            {
                Id = 1,
                Name = "John Doe",
                Address = "123 Main",
                Email2 = "New York",
                Phone = "123-456-7890"
            });

        var customerService = new CustomerService(customerDal.Object, customerRule.Object);
        var customer = await customerService.GetCustomerByNameAndPhoneAsync("John Doe", "123-456-7890");

        Assert.NotNull(customer);
        Assert.Equal(1, customer.Id);
        Assert.Equal("John Doe", customer.Name);
        Assert.Equal("123 Main", customer.Address);
        Assert.Equal("New York", customer.Email2);
        Assert.Equal("123-456-7890", customer.Phone);
        customerDal.Verify(x => x.GetCustomerByNameAndPhone("John Doe", "123-456-7890"), Times.Once);
    }

    [Fact]
    public async Task GetCustomerByNameAndPhoneAsync_Null()
    {
        var customerDal = new Mock<ICustomerRepository>();
        var customerRule = new Mock<ICustomerRuler>();
        customerDal.Setup(x => x.GetCustomerByNameAndPhone(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(null as Customer);

        var customerService = new CustomerService(customerDal.Object, customerRule.Object);
        var customer = await customerService.GetCustomerByNameAndPhoneAsync("Jane Doe", "123-456-7891");

        Assert.Null(customer);
        customerDal.Verify(x => x.GetCustomerByNameAndPhone("Jane Doe", "123-456-7891"), Times.Once);
    }
}