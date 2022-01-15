using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using dotNet101.Data;
using dotNet101.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace dotNet101.IntegrationTest
{
    public class DotNet101APITest
    {
        private readonly HttpClient _client;
        private readonly string _url = "api/Students";
        private readonly ApplicationDbContext _context;

        public DotNet101APITest()
        {
            var server = new TestServer(new WebHostBuilder().UseEnvironment("Test").UseStartup<Startup>());
            _context = server.Host.Services.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
            Seed();
            _client = server.CreateClient();
        }

        private void Seed()
        {
            if (_context.Database.EnsureCreated())
            {
                _context.Students.AddRangeAsync(new[] { new Student { Name = "Itadori", Grade = "unknown" }, new Student { Name = "Magumi", Grade = "B" } });
                _context.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task TestGetAllStudentsAsyncThenReturnOkStatus()
        {
            var response = await _client.GetAsync(_url);

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestGetStudentAsyncThenReturnOkStatus()
        {
            var response = await _client.GetAsync($"{_url}/{1}");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestGetStudentAsyncThenReturnNotFound()
        {
            var response = await _client.GetAsync(_url + 5);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task TestPostStudentAsync()
        {
            var json = JsonConvert.SerializeObject(new Student() { Name = "Satoro Gojo", Grade = "Special" });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_url, content);
            var jsonFromPostResponse = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var singleResponse = JsonConvert.DeserializeObject<Student>(jsonFromPostResponse);
            await _client.DeleteAsync($"/{_url}/{singleResponse.StudentId}");
        }

        [Fact]
        public async Task TestPostStudentAsyncThenReturnBadRequest()
        {
            var json = JsonConvert.SerializeObject(new Student() { Name = "", Grade = "Special" });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_url, content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task TestPutStudentAsyncSuccessfullyThenReturnNoContent()
        {
            Student student = new Student() { StudentId = 1, Name = "Itadori", Grade = "Special" };
            var json = JsonConvert.SerializeObject(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/{_url}/{student.StudentId}", content);

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async void TestDeleteStudentAsyncSuccessfullyThenReturnNoContent()
        {
            var json = JsonConvert.SerializeObject(new Student() { Name = "Satoro Gojo", Grade = "Special" });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_url, content);
            var jsonFromPostResponse = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            var singleResponse = JsonConvert.DeserializeObject<Student>(jsonFromPostResponse);
            var deleteResponse = await _client.DeleteAsync($"/{_url}/{singleResponse.StudentId}");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            deleteResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void TestDeleteStudentAsyncSuccessfullyThenReturnNotFound()
        {
            var deleteResponse = await _client.DeleteAsync($"/{_url}/5");

            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
        }
    }
}
