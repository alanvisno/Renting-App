using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using TestCore.Api.Models.Car;

namespace TesCore.Test.Integration.Car
{
    [TestClass]
    public class CarApiTest
    {
        private readonly HttpClient _httpClient;
        private readonly TokenHelper _tokenHelper;

        private const string _carListUrl = "/api/Car/List";
        private const string _carCreateUrl = "/api/Car/Create";

        public CarApiTest()
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
        [DataRow("2023-05-01", "2023-05-02")]
        public async Task Car_List_Success(string startDate, string endDate)
        {
            var request = new CarListRequest { 
                StartDate = DateTime.Parse(startDate), EndDate = DateTime.Parse(endDate) };

            var response = await _httpClient.PostAsJsonAsync(_carListUrl, request);
            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<CarListResponse>();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.List.Any());
        }

        [TestMethod]
        [DataRow("Car Example", 25)]
        public async Task Car_Create_Success(string description, int price)
        {
            var request = new CarCreateRequest
            {
                Description = description,
                PatentDate = DateTime.Now.AddYears(-4).AddMonths(-11), //5 years top
                PricePerDay = price //more than 0
            };
            var jsonRequest = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_carListUrl, httpContent);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [DataRow("car example", 0)]
        [DataRow(null, 25)]
        [DataRow(null, 0)]
        [DataRow("", 25)]
        [DataRow("", 0)]
        public async Task Car_Create_GoodDate_Fail(string description, int price)
        {
            var request = new CarCreateRequest
            {
                Description = description,
                PricePerDay = price,
                PatentDate = DateTime.Now
            };

            var response = await _httpClient.PostAsJsonAsync(_carCreateUrl, request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        [DataRow("Car Example", 25)]
        public async Task Car_Create_BadDate_Fail(string description, int price)
        {
            var request = new CarCreateRequest
            {
                Description = description,
                PricePerDay = price, //more than 0
                PatentDate = DateTime.Now.AddYears(-5)
            };

            var response = await _httpClient.PostAsJsonAsync(_carCreateUrl, request);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
