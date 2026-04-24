namespace CSharpApi.Models;

public class NetworkDevice
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public bool IsOnline { get; set; }
}