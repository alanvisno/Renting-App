using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using TesCore.Test.Integration;
using TestCore.Api.Models.Customer;

namespace TestCore.Test.Customer
{
    [TestClass]
    public class CustomerApiTest
    {
        private readonly HttpClient _httpClient;
        private readonly TokenHelper _tokenHelper;

        private const string _customerListUrl = "/api/Customer/Search";

        public CustomerApiTest()
        {
            var app = AppHelper.ConfigureApp();
            _httpClient = app.CreateClient();
            _tokenHelper = new TokenHelper(_httpClient, app);
        }

        [TestInitialize]
        public async Task InitializeAsync()
        {
            var bearerToken = await _tokenHelper.GetBearerTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        public async Task Customer_Search_Fail(string name)
        {
            var request = new CustomerSearchRequest { Name = name };
            var jsonRequest = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_customerListUrl, httpContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [DataRow("alan")]
        public async Task Customer_Search_Sucess(string name)
        {
            var request = new CustomerSearchRequest { Name = name };
            var jsonRequest = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_customerListUrl, httpContent);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<CustomerListResponse>();
            Assert.IsNotNull(result);
        }
    }
}
