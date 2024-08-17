using System.Diagnostics.CodeAnalysis;
using Agency.AgencyModule.Repos;
using Agency.AgencyModule.Services;
using Agency.AppointmentModule.Repos;
using Agency.AppointmentModule.Services;
using Agency.CustomerModule.Models;
using Agency.CustomerModule.MsgConsumer;
using Agency.CustomerModule.Repository;
using Agency.CustomerModule.Services;
using Agency.CustomerModule.Validators;

namespace Agency;

[ExcludeFromCodeCoverage]
internal static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.GetConfig();
        builder.ShowInfoApp();

        builder.SetupServiceSystem(queryLocation:"./Queries/blueprint.json");
        builder.SetupThirdPartyObjectByConfig();

        builder.Services.AddAutoMapper(typeof(CustomerMapper));
        builder.Services.AddTransient<ICustomerService, CustomerService>();
        builder.Services.AddTransient<ICustomerRuler, CustomerRuler>();
        builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
        
        builder.Services.AddTransient<IAppointmentService, AppointmentService>();
        builder.Services.AddTransient<IAppointmentRepository, AppointmentRepository>();
        
        builder.Services.AddTransient<IAgencyService, AgencyService>();
        builder.Services.AddTransient<IAgencyRepository, AgencyRepository>();
        
        builder.Services.AddTransient<IAgencySettingService, AgencySettingService>();
        builder.Services.AddTransient<IAgencySettingRepository, AgencySettingRepository>();

        builder.Services.AddHostedService<SyncCustomer2>();

        var app = builder.Build();
        app.SetupApp(pathHealthCheck:"/blueprint/healthchecks");
        app.Run();
    }
}