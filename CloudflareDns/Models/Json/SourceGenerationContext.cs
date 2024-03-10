using CloudflareDns.Models;
using System.Text.Json.Serialization;

namespace CloudflareDns;

[JsonSourceGenerationOptions(WriteIndented = true, PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(ZoneIdResult))]
[JsonSerializable(typeof(DnsRecordsResult))]
[JsonSerializable(typeof(DnsRecordRequest))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}