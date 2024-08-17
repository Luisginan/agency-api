using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Core.Base;

[ExcludeFromCodeCoverage]
public class ServiceBase
{
    protected static T? GetData<T>(string url) where T : class
    {
        T? result;
        using var client = new HttpClient();
        client.BaseAddress = new Uri(url);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var response = client.GetAsync(url).Result;
        if (response.IsSuccessStatusCode)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;
            result = JsonConvert.DeserializeObject<T>(responseString);
        } else
        {
            throw new Exception("Error while fetching data from API " + response.StatusCode);
        }

        return result;
    }
    public string PostData(string url, string data)
    {
        var result = string.Empty;
        using var client = new HttpClient();
        client.BaseAddress = new Uri(url);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var response = client.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json")).Result;
        if (response.IsSuccessStatusCode)
        {
            result = response.Content.ReadAsStringAsync().Result;
        }
        return result;
    }

    //put data to api
    public string PutData(string url, string data)
    {
        var result = string.Empty;
        using var client = new HttpClient();
        client.BaseAddress = new Uri(url);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var response = client.PutAsync(url, new StringContent(data, Encoding.UTF8, "application/json")).Result;
        if (response.IsSuccessStatusCode)
        {
            result = response.Content.ReadAsStringAsync().Result;
        }

        return result;
    }

    //delete data from api
    public string DeleteData(string url)
    {
        var result = string.Empty;
        using var client = new HttpClient();
        client.BaseAddress = new Uri(url);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var response = client.DeleteAsync(url).Result;
        if (response.IsSuccessStatusCode)
        {
            result = response.Content.ReadAsStringAsync().Result;
        }

        return result;
    }
}