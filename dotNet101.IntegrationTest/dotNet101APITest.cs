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
        private readonly string _url = "api/Students/";   
        public dotNet101APITest()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development").UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Fact]
        public async Task GetAllStudentsThenReturnOkStautus()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/Students/");

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetStudentThenReturnOkStatus()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/Students/" + 1);

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostNewStudentAndDeleteAfter()
        {

            var json = JsonConvert.SerializeObject(new Student() { Name = "Satoro Gojo", Grade = "Special" });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(_client.BaseAddress + "api/Students"),
                Method = HttpMethod.Post,
                Content = content
            };

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var deleteResponse = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, _url + response.Headers.Location.Segments[3]));

            deleteResponse.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }


        [Fact]
        public async Task PutStudentSuccessfullyThenReturnNoContent()
        {
            var json = JsonConvert.SerializeObject(new Student() { StudentId = 1, Name = "Itadori", Grade = "Special"});
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(_client.BaseAddress+"api/Students/1"),
                Method = HttpMethod.Put,
                Content = content
            };

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
