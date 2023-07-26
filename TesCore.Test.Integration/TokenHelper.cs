using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;

namespace TesCore.Test.Integration
{
    public class TokenHelper
    {
        private HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public TokenHelper(HttpClient httpClient, WebApplicationFactory<Program> app)
        {
            _httpClient = httpClient;
            _configuration = app.Services.GetRequiredService<IConfiguration>();
        }

        public async Task<string> GetBearerTokenAsync()
        {
            var username = _configuration["TestCredentials:Username"];
            var password = _configuration["TestCredentials:Password"];

            var credentials = new { userName = username, password = password };
            var jsonCredentials = JsonSerializer.Serialize(credentials);
            var httpContent = new StringContent(jsonCredentials, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/identity/token", httpContent);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var tokenObject = JsonSerializer.Deserialize<JsonElement>(responseBody);
            return tokenObject.GetProperty("accessToken").GetString() ?? "";
        }
    }
}
