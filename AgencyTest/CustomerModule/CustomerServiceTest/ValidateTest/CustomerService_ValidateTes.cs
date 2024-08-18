using AgencyApi.CustomerModule.Models;
using AgencyApi.CustomerModule.Repository;
using AgencyApi.CustomerModule.Services;
using AgencyApi.CustomerModule.Validators;
using Moq;

namespace AgencyTest.CustomerServiceTest.ValidateTest;

public class CustomerServiceValidateTes
{
    //create test
    [Fact]
    public void ValidateCustomer_Success()
    {
        var customerDal = new Mock<ICustomerRepository>();
        var customerRule = new CustomerRuler();
        customerDal.Setup(x => x.GetCustomerByEmail(It.IsAny<string>())).Returns((Customer?)null);

        var customerService = new CustomerService(customerDal.Object, customerRule);
        var customer = new Customer
        {
            Name = "",
            Address = "",
            Email2 = "",
            Phone = ""
        };

        var error = customerService.Validate(customer);
        Assert.NotNull(error);
        Assert.NotEmpty(error);
        Assert.Equal("StringTooShort: #/name", error[0]);
        Assert.Equal("StringTooShort: #/email", error[1]);
        Assert.Equal("EmailExpected: #/email", error[2]);
        Assert.Equal("StringTooShort: #/phone", error[3]);
        Assert.Equal("StringTooShort: #/address", error[4]);
    }
    
}