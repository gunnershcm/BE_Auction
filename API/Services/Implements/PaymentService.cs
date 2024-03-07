//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using Domain.Mails;
//using Microsoft.Extensions.Options;
//using Newtonsoft.Json;
//using PayPalCheckoutSdk.Core;
//using PayPalCheckoutSdk.Orders;
//using PayPalHttp;
//using HttpClient = PayPalHttp.HttpClient;
//using HttpResponse = PayPalHttp.HttpResponse;

//namespace API.Services.Implements
//{
//    public class PaymentService : IPaymentService
//    {
//        private readonly PaymentSettings _paymentSettings;
//        private readonly HttpClient _httpClient;

//        public PaymentService(IOptions<PaymentSettings> paymentSettings, HttpClient httpClient)
//        {
//            _paymentSettings = paymentSettings.Value;
//            _httpClient = httpClient;
//            _httpClient.BaseAddress = new Uri("https://api.sandbox.paypal.com"); // Use Sandbox environment
//        }

//        public async Task<HttpResponse> CreateOrderAsync(decimal amount, string currency)
//        {
//            // Get access token
//            var accessToken = await GetAccessTokenAsync();

//            // Build request body
//            var request = new
//            {
//                intent = "CAPTURE",
//                purchase_units = new[]
//                {
//                    new
//                    {
//                        amount = new
//                        {
//                            currency_code = currency,
//                            value = amount.ToString("0.00")
//                        }
//                    }
//                }
//            };

//            // Convert request to JSON
//            var jsonRequest = JsonConvert.SerializeObject(request);

//            // Send request to PayPal API
//            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/v2/checkout/orders");
//            httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
//            httpRequest.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

//            return await _httpClient.SendAsync(httpRequest);
//        }

//        private async Task<string> GetAccessTokenAsync()
//        {
//            // Build token request
//            var request = new HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token");
//            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
//                "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_paymentSettings.ClientId}:{_paymentSettings.ClientSecret}")));

//            request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

//            // Send token request and receive response
//            var response = await _httpClient.SendAsync(request);

//            // Read token from response
//            var responseContent = await response.Content.ReadAsStringAsync();
//            dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);
//            return jsonResponse.access_token;
//        }

//        public async Task<HttpResponse> CaptureOrderAsync(string orderId)
//        {
//            // Get access token
//            var accessToken = await GetAccessTokenAsync();

//            // Send request to capture order
//            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"/v2/checkout/orders/{orderId}/capture");
//            httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
//            httpRequest.Content = new StringContent("{}", Encoding.UTF8, "application/json");

//            return await _httpClient.SendAsync(httpRequest);
//        }
//    }
//}
