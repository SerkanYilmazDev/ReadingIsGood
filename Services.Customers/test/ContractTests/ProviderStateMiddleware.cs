using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Services.Customers.Data;

namespace ContractTests
{
    public class ProviderStateMiddleware
    {
        private readonly RequestDelegate _next;
        public ProviderStateMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var basket = new Basket();
            basket.CustomerId = new Guid("0a5f9589-e0d7-4c2a-a9d2-941a2f5e650b");
            basket.Items = new List<BasketItem>();
            basket.Items.Add(new BasketItem { ProductName = "" });

            var serializedBasket = JsonConvert.SerializeObject(basket, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync(serializedBasket);
        }
    }
}