using CloudflareDns.Models;
using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;

namespace CloudflareDns;

internal class CloudFlareDnsClient
{
    private const string baseAddress = "https://api.cloudflare.com/client/v4/zones/";
    private static SourceGenerationContext Types => SourceGenerationContext.Default;

    private readonly HttpClient _client;
    private readonly DnsClientSettings _settings;
    private string zoneId = "";

    public CloudFlareDnsClient(DnsClientSettings settings)
    {
        _settings = settings;
        _client = new()
        {
            BaseAddress = new Uri(baseAddress)
        };
        _client.DefaultRequestHeaders.Authorization = new("Bearer", _settings.Token);
    }

    private async Task GetZoneId()
    {
        if(zoneId != "")
        {
            return;
        }
        var result = await _client.GetFromJsonAsync($"?name={_settings.Zone}", Types.ZoneIdResult);
        zoneId = result?.Result.FirstOrDefault()?.Id ?? "";
    }

    public async Task CreateRecord(string name, string ip, string type = "A")
    {
        var request = new DnsRecordRequest(name, ip, type);
        await PostAsync($"dns_records", request, Types.DnsRecordRequest);
    }

    public async Task UpdateRecord(string id, string name, string ip, string type = "A")
    {
        var request = new DnsRecordRequest(name, ip, type);
        await PatchAsync($"dns_records/{id}", request, Types.DnsRecordRequest);
    }

    public async Task<DnsRecord?> GetRecord(string name)
    {
        await GetZoneId();
        var result = await GetAsync($"dns_records?name={name}.{_settings.Zone}", Types.DnsRecordsResult);
        return result?.Result.FirstOrDefault();
    }

    private async Task<T?> GetAsync<T>(string? requestUri, JsonTypeInfo<T> jsonTypeInfo)
    {
        await GetZoneId();
        return await _client.GetFromJsonAsync($"{zoneId}/{requestUri}", jsonTypeInfo);
    }

    private async Task PatchAsync<T>(string? requestUri, T request, JsonTypeInfo<T> jsonTypeInfo)
    {
        await GetZoneId();
        await _client.PatchAsJsonAsync($"{zoneId}/{requestUri}", request, jsonTypeInfo);
    }

    private async Task PostAsync<T>(string? requestUri, T request, JsonTypeInfo<T> jsonTypeInfo)
    {
        await GetZoneId();
        await _client.PostAsJsonAsync($"{zoneId}/{requestUri}", request, jsonTypeInfo);
    }
}
