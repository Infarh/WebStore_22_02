using System.Net.Http.Json;

using WebStore.Interfaces.TestAPI;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Values;

public class ValuesClient : BaseClient, IValuesService
{
    public ValuesClient(HttpClient Client) : base(Client, "api/values") { }

    // ReSharper disable once UnusedParameter.Local
    private static IEnumerable<T> ReadAsTT<T>(HttpContent content, T _)
    {
        return content.ReadFromJsonAsync<IEnumerable<T>>().Result!;
    }

    public IEnumerable<string> GetValues()
    {
        var response = Http.GetAsync(Address).Result;
        if (response.IsSuccessStatusCode)
            // ReSharper disable once HeuristicUnreachableCode
#pragma warning disable CS0162
            return ReadAsTT(response.Content, true ? null : new { Key = 0, Value = "123" }).Select(v => v.Value);
#pragma warning restore CS0162

        return Enumerable.Empty<string>();
    }

    public int Count()
    {
        var response = Http.GetAsync($"{Address}/count").Result;
        if (response.IsSuccessStatusCode)
            return response.Content.ReadFromJsonAsync<int>().Result;

        return -1;
    }

    public string? GetById(int id)
    {
        var response = Http.GetAsync($"{Address}/{id}").Result;
        if (response.IsSuccessStatusCode)
            return response.Content.ReadFromJsonAsync<string>().Result;
        return null;
    }

    public void Add(string Value)
    {
        var response = Http.PostAsJsonAsync(Address, Value).Result;
        response.EnsureSuccessStatusCode();
    }

    public void Edit(int id, string value)
    {
        var response = Http.PutAsJsonAsync($"{Address}/{id}", value).Result;
        response.EnsureSuccessStatusCode();
    }

    public bool Delete(int id)
    {
        var response = Http.DeleteAsync($"{Address}/{id}").Result;
        return response.IsSuccessStatusCode;
    }
}
