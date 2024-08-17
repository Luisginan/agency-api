using Agency.CustomerModule.Models;

namespace Agency.CustomerModule.Validators;

public interface ICustomerRuler
{
    List<string> Validate(Customer customer);
}