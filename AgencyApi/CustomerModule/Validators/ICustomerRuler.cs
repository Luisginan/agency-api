using AgencyApi.CustomerModule.Models;

namespace AgencyApi.CustomerModule.Validators;

public interface ICustomerRuler
{
    List<string> Validate(Customer customer);
}