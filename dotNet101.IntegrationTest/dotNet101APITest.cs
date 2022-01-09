using dotNet101.IntegrationTest.Attributes;
using dotNet101.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dotNet101.IntegrationTest
{
    public class dotNet101APITest
    {
        private readonly HttpClient _client;
        private readonly string _url = "api/Students";
        public dotNet101APITest()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development").UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Fact]
        public async Task TestGetAllStudentsAsyncThenReturnOkStatus()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _url);
            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestGetStudentAsyncThenReturnOkStatus()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/Students/" + 1);
            var response = await _client.SendAsync(request);

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

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, _url + response.Headers.Location.Segments[3]));
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
