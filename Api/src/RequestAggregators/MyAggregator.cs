// using System;
// using Ocelot.Middleware;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using System.Collections.Generic;
// using Ocelot.Multiplexer;
// using Newtonsoft.Json.Linq;
// using Newtonsoft.Json;
// using System.Net.Http;
// using System.Net;
// using System.Net.Http.Headers;

// namespace Api.RequestAggregators
// {
//     public partial class Startup
//     {
//         public class MyAggregator : IDefinedAggregator
//         {
//             public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
//             {
//                 JObject responseObject = null;
//                 JsonMergeSettings mergeSetting = new JsonMergeSettings();
//                 mergeSetting.MergeArrayHandling = MergeArrayHandling.Union;
//                 mergeSetting.PropertyNameComparison = System.StringComparison.CurrentCultureIgnoreCase;

//                 foreach (var httpResponse in responses)
//                 {
//                     var content = await httpResponse.Items.DownstreamResponse().Content.ReadAsStringAsync();
//                     var contentObject = JsonConvert.DeserializeObject(content);
//                     if (responseObject == null)
//                     {
//                         responseObject = JObject.FromObject(contentObject);
//                     }
//                     else
//                     {
//                         responseObject.Merge(contentObject, mergeSetting);
//                     }
//                 }
//                 var stringContent = new StringContent(responseObject.ToString())
//                 {
//                     Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
//                 };

//                 return new DownstreamResponse(stringContent, HttpStatusCode.OK,
//                 new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");
//             }
//         }
//     }
// }
