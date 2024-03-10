using CloudflareDns;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddJsonFile("config.json", optional: true)
    .AddUserSecrets<CloudFlareDnsClient>()
    .AddCommandLine(args);

var config = builder.Build().Get<DnsUpdateSettings>();
if(config is null)
{
    Console.WriteLine("No config.");
    return;
}

var client = new CloudFlareDnsClient(config);
var record = await client.GetRecord(config.Record);
if (record is null)
{
    Console.WriteLine("Creating record {0}...", config.Record);
    await client.CreateRecord(config.Record, config.Ip);
}
else if (record.Locked)
{
    Console.WriteLine("Record {0} is locked, cannot update.", config.Record);
}
else if (record.Content != config.Ip)
{
    Console.WriteLine("Updating record {0}...", config.Record);
    await client.UpdateRecord(record.Id, record.Name, config.Ip);
}
else
{
    Console.WriteLine("Record {0} is already set to {1}.", config.Record, config.Ip);
}
