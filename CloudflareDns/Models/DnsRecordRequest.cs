namespace CloudflareDns.Models;

internal record DnsRecordRequest(string Name, string Content, string Type);
