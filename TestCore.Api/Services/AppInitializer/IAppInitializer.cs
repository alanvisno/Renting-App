namespace TestCore.Api.Services
{
    public interface IAppInitializer
    {
        void SetUserDataMapping(WebApplication app);
        Task Initialize(WebApplication app);
        void SetTokenMapping(WebApplication app, WebApplicationBuilder builder);
    }
}
