using eIDEAS.Data;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;
using System.Net;
using Xunit;
using AngleSharp;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace eIDEAS.UnitTests
{

    public class BasicTests
    : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public BasicTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        //[Theory]
        //[InlineData("/")]
        //[InlineData("/Index")]
        //[InlineData("/About")]
        //[InlineData("/Privacy")]
        //[InlineData("/Contact")]
        //[InlineData("/Register")]
        //[InlineData("/Login")]
        
        [Fact]
        public async Task PTestAsync()
        {
            string url = "/identity/Account/Login";
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            //response.IsSuccessStatusCode.IsTrue();
        }
        [Fact]
        public async Task Should_be_able_to_see_secret()
        {
            var data = new Dictionary<string, string>()
    {
        { "Email", "Jennifer@HealthEsteem.ca" },
        { "Password", "Abc123!" }
    };
            string url = "/identity/Account/Login";
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var secretContent = new FormUrlEncodedContent(data);
            try
            {
                string xx = JsonConvert.SerializeObject(secretContent);
                //JsonSerializer.Serialize( jsonWriter, secretContent);
                byte[] mm = System.Text.Encoding.UTF8.GetBytes(xx);
                var cont = new ByteArrayContent(mm);
                cont.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var result = await client.PostAsync("/identity/Account/Login", cont);
             Assert.True(result.Headers.Location.ToString()==(""));
            }
            catch (Exception x)
            {

            }
            //client.DefaultRequestHeaders.Add("my-name", "test");
            //client.DefaultRequestHeaders.Add("my-id", "12345");
            
           
        }

    }
    }
