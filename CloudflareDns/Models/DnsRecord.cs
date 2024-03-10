namespace CloudflareDns.Models;

internal record DnsRecord(string Id, string Name, string Type, bool Locked, bool Proxied, string Content);
