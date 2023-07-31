using System.Net.Http.Json;
using System.Net;
using TestCore.Api.Models.Rent;
using System.Net.Http.Headers;

namespace TesCore.Test.Integration.Rent
{
    [TestClass]
    public class RentApiTest
    {
        private HttpClient _httpClient;
        private TokenHelper _tokenHelper;

        public RentApiTest()
        {
            var app = AppHelper.GetAppWithSettings();
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
            var customerId = Guid.NewGuid();
            var request = new RentListRequest { CustomerId = customerId };
            var response = await _httpClient.PostAsJsonAsync("/api/Rent/List", request);

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
                CustomerId = new Guid("9f0a285b-76f3-4705-ac5a-e3a9806fb2f4"),
                CarId = new Guid("04a1799e-8e3b-4b20-81ad-54abf7532d59"),
                DeliveryDate = DateTime.Now.AddDays(1),
                ReturnDate = DateTime.Now.AddDays(7),
                PricePerDay = 45
            };
            var response = await _httpClient.PostAsJsonAsync("/api/Rent/Create", request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Create_Rent_Fail()
        {
            var request = new RentCreateRequest
            {
                //Empty
            };
            var response = await _httpClient.PostAsJsonAsync("/api/Rent/Create", request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Create_Rent_NotFound()
        {
            var request = new RentCreateRequest
            {
                CustomerId = Guid.NewGuid(),
                CarId = Guid.NewGuid(),
                DeliveryDate = DateTime.Now.AddDays(1),
                ReturnDate = DateTime.Now.AddDays(7),
                PricePerDay = 45
            };
            var response = await _httpClient.PostAsJsonAsync("/api/Rent/Create", request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public async Task Return_Car_Success()
        {
            var request = new RentReturnRequest
            {
                RentId = new Guid("d52368e5-b829-46ba-8f5e-2d31d13946ed"),
                ReturnDate = DateTime.Now
            };
            var response = await _httpClient.PutAsJsonAsync("/api/Rent/Return", request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Return_Car_NotFound()
        {
            var request = new RentReturnRequest
            {
                RentId = Guid.NewGuid(),
                ReturnDate = DateTime.Now
            };
            var response = await _httpClient.PutAsJsonAsync("/api/Rent/Return", request);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
