using System.Net.Http.Json;
using System.Net;
using TestCore.Api.Models.Rent;
using System.Net.Http.Headers;

namespace TesCore.Test.Integration.Rent
{
    [TestClass]
    public class RentApiTest
    {
        private readonly HttpClient _httpClient;
        private readonly TokenHelper _tokenHelper;

        private const string _rentListUrl = "/api/Rent/List";
        private const string _rentCreateUrl = "/api/Rent/Create";
        private const string _rentReturnUrl = "/api/Rent/Return";

        public RentApiTest()
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
        public async Task Rent_List_Sucess()
        {
            var customerId = 564564;
            var request = new RentListRequest { CustomerId = customerId };
            var response = await _httpClient.PostAsJsonAsync(_rentListUrl, request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<RentListResponse>();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Rents != null);
        }

        [TestMethod]
        public async Task Create_Rent_Success()
        {
            var request = new RentCreateRequest
            {
                CustomerId = 123,
                CarId = 1234,
                DeliveryDate = DateTime.Now.AddDays(1),
                ReturnDate = DateTime.Now.AddDays(7),
                PricePerDay = 45
            };
            var response = await _httpClient.PostAsJsonAsync(_rentCreateUrl, request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Create_Rent_Fail()
        {
            var request = new RentCreateRequest
            {
                //Empty
            };
            var response = await _httpClient.PostAsJsonAsync(_rentCreateUrl, request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Create_Rent_NotFound()
        {
            var request = new RentCreateRequest
            {
                CustomerId = 123,
                CarId = 1234,
                DeliveryDate = DateTime.Now.AddDays(1),
                ReturnDate = DateTime.Now.AddDays(7),
                PricePerDay = 45
            };
            var response = await _httpClient.PostAsJsonAsync(_rentCreateUrl, request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Return_Car_Success()
        {
            var request = new RentReturnRequest
            {
                RentId = 1235,
                ReturnDate = DateTime.Now
            };
            var response = await _httpClient.PutAsJsonAsync(_rentReturnUrl, request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Return_Car_NotFound()
        {
            var request = new RentReturnRequest
            {
                RentId = 1235,
                ReturnDate = DateTime.Now
            };
            var response = await _httpClient.PutAsJsonAsync(_rentReturnUrl, request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
