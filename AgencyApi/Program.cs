using System.Diagnostics.CodeAnalysis;
using AgencyApi.AgencyModule.Models;
using AgencyApi.CustomerModule.MsgConsumer;
using AgencyApi.AgencyModule.Repos;
using AgencyApi.AgencyModule.Services;
using AgencyApi.AppointmentModule.Models;
using AgencyApi.AppointmentModule.Repos;
using AgencyApi.AppointmentModule.Services;
using AgencyApi.CustomerModule.Models;
using AgencyApi.CustomerModule.Repository;
using AgencyApi.CustomerModule.Services;
using AgencyApi.CustomerModule.Validators;
using AgencyApi.TokenIssuanceModule.Repos;
using AgencyApi.TokenIssuanceModule.Services;

namespace AgencyApi;

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

        builder.Services.AddAutoMapper(typeof(CustomerMapper), typeof(AppointmentMapper), typeof(AgencyMapper));
        
        builder.Services.AddTransient<ICustomerService, CustomerService>();
        builder.Services.AddTransient<ICustomerRuler, CustomerRuler>();
        builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
        
        builder.Services.AddTransient<IAppointmentService, AppointmentService>();
        builder.Services.AddTransient<IAppointmentRepository, AppointmentRepository>();
        
        builder.Services.AddTransient<IAgencyService, AgencyService>();
        builder.Services.AddTransient<IAgencyRepository, AgencyRepository>();
        
        builder.Services.AddTransient<IAgencySettingService, AgencySettingService>();
        builder.Services.AddTransient<IAgencySettingRepository, AgencySettingRepository>();
        
        builder.Services.AddTransient<ITokenIssuanceService, TokenIssuanceService>();
        builder.Services.AddTransient<ITokenIssuanceRepository, TokenIssuanceRepository>();
        

        var app = builder.Build();
        app.SetupApp(pathHealthCheck:"/blueprint/healthchecks");
        app.Run();
    }
}