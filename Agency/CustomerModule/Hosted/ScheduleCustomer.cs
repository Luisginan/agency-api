using System.Diagnostics.CodeAnalysis;
using Agency.CustomerModule.Models;
using Agency.CustomerModule.Services;
using Core.Base;

namespace Agency.CustomerModule.Hosted;

[ExcludeFromCodeCoverage]
public class CustomerScheduler(ILogger<CustomerScheduler> logger,
    ILogger<ProducerBase> baseLogger, 
    IServiceProvider serviceProvider) :
    SchedulerBase<Customer>(baseLogger)
{

    private ICustomerService? _customerService;

    protected override void OnPrepare()
    {
        GetCustomerService();
    }

    protected override List<Customer> GetData()
    {
        var result = _customerService?.GetListCustomer();
        logger.LogInformation("SchedulerCustomer GetData successful: {info}", result?.Count);
        return result ?? [];
    }

    private void GetCustomerService()
    {
        var scope = serviceProvider.CreateScope();
        _customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();
    }

    protected override void OnExecuteRequest(Customer item)
    {
        item.IsActive = true;
        _customerService?.UpdateCustomer(item, item.Id);
        logger.LogInformation("SchedulerCustomer OnExecuteRequest successful: {info}", "Customer updated");
    }

    protected override int Delay()
    {
        return 10000;
    }
}