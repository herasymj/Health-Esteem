using eIDEAS.Data;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;
using System.Net;
using Xunit;
using AngleSharp;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;

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
        
            
        }
    }
