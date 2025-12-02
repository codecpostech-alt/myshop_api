using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class ChargilyPaymentService
{
    private readonly HttpClient _httpClient;
    private readonly string _secretKey;

    public ChargilyPaymentService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _secretKey = config["Chargily:SecretKey"];

        if (string.IsNullOrWhiteSpace(_secretKey))
            throw new Exception("Chargily SecretKey not found in configuration!");
    }

    public async Task<string> CreateCheckoutAsync(
        decimal amount,
        string plan,
        string successUrl,
        string failureUrl)
    {
        // جسم الطلب
        var request = new
        {
            amount = (int)(amount * 100), // بالـ centimes
            currency = "dzd",             // يجب أن يكون حرف صغير
            payment_method = "edahabia",  // أو "cib" أو "chargily_app"
            success_url = successUrl,
            failure_url = failureUrl,
            locale = "ar",
            metadata = new { plan = plan },
            // تم حذف حقل customer لأنه غير مدعوم الآن
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // تهيئة HttpClient وإرسال المفتاح بطريقة صحيحة
        var httpRequest = new HttpRequestMessage(HttpMethod.Post,
            "https://pay.chargily.net/test/api/v2/checkouts");
        httpRequest.Content = content;
        httpRequest.Headers.Clear();
        httpRequest.Headers.Add("Authorization", "Bearer " + _secretKey);
        var response = await _httpClient.SendAsync(httpRequest);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception("Chargily API Error: " + responseString);

        using var doc = JsonDocument.Parse(responseString);
        return doc.RootElement.GetProperty("checkout_url").GetString();
    }
}