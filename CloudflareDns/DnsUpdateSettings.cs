namespace CloudflareDns;

internal record DnsUpdateSettings(string Record, string Ip, string Zone, string Token) : DnsClientSettings(Zone, Token);
