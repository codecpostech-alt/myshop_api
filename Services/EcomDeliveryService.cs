using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyShop.Services
{
    public class EcomDeliveryService
    {
        private readonly HttpClient _client;

        public EcomDeliveryService(string apiKey, string token)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://ecom-dz.net/Api_v1/")
            };
            _client.DefaultRequestHeaders.Add("Key", apiKey);
            _client.DefaultRequestHeaders.Add("Token", token);
        }

        /// <summary>
        /// يتحقق إن كان حساب ECOM DELIVERY مفعل وصحيح.
        /// </summary>
        public async Task<bool> IsActivatedAsync()
        {
            try
            {
                var response = await _client.GetAsync("Test");
                if (!response.IsSuccessStatusCode)
                    return false;

                var content = await response.Content.ReadAsStringAsync();
                return content.Contains("Fournisseur"); // علامة نجاح
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}