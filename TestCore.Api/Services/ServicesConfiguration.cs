using FluentValidation;
using TestCore.Api.Exceptions.Middleware;
using TestCore.Api.Models.Car;
using TestCore.Api.Models.Rent;
using TestCore.Api.Validators;
using TestCore.Api.Validators.CarValidators;
using TestCore.Api.Validators.RentValidators;
using TestCore.Business;
using TestCore.Business.CustomerServices;
using TestCore.Data;

namespace TestCore.Api.Services
{
    public static class ServicesConfiguration
    {
        //Dependency Injection
        public static void ConfigureIOC(this IServiceCollection services)
        {
            //Services
            services.AddScoped<ICarServices, CarServices>();
            services.AddScoped<IRentServices, RentServices>();
            services.AddScoped<ICustomerServices, CustomerServices>();

            //Validators
            services.AddScoped<IValidator<RentCreateRequest>, RentCreateValidator>();
            services.AddScoped<IValidator<CarCreateRequest>, CarCreateValidator>();
            services.AddSingleton<ICarCreateValidatorBase, CarCreateValidatorBase>();

            //Main Initializers
            services.AddTransient<IAppInitializer, AppInitializer>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICoreDataContext, CoreDataContext>();

            services.AddTransient<ExceptionMiddleware>();
            services.AddMvc();
        }
    }
}