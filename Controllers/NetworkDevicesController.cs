using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using CSharpApi.Data;
using CSharpApi.Models;
using System.Net;

namespace CSharpApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class NetworkDevicesController : ControllerBase
{
    private readonly AppDbContext _context;

    public NetworkDevicesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<NetworkDevice>>> GetDevices()
    {
        return await _context.Devices.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<NetworkDevice>> PostDevice(NetworkDevice device)
    {
        _context.Devices.Add(device);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDevices), new { id = device.Id }, device);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutDevice(int id, NetworkDevice device)
    {
        if (id != device.Id)
        {
            return BadRequest("ID w adresie musi się zgadzać z ID w ciele zapytania");
        }

        _context.Entry(device).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Devices.Any(e => e.Id == id))
            {
                return NotFound("Nie znalieziono urządzenia o podanym ID");
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevice(int id)
    {
        var device = await _context.Devices.FindAsync(id);
        if (device == null)
        {
            return NotFound("Nie znaleziono urządzenia o podanym ID");
        }

        _context.Devices.Remove(device);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("scan")]
    public async Task<IActionResult> ScanNetwork([FromQuery] string baseIp = "192.168.0.")
    {
        var tasks = new List<Task<NetworkDevice?>>();

        for (int i = 1; i < 255; i++)
        {
            string ip = $"{baseIp}{i}";
            tasks.Add(PingDeviceAsync(ip));
        }

        var results = await Task.WhenAll(tasks);

        var activeDevices = results.Where(d => d != null).ToList();

        foreach(var device in activeDevices)
        {
            if(!_context.Devices.Any(d => d.IpAddress == device!.IpAddress))
            {
                _context.Devices.Add(device!);
            }
        }
        await _context.SaveChangesAsync();

        return Ok($"Skanowanie zakończone. Znaleziono aktywnych urządzeń: {activeDevices.Count}");
    }

    private async Task<NetworkDevice?> PingDeviceAsync(string ip)
    {
        try 
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(ip, 200);
        
            if (reply.Status == IPStatus.Success)
            {
                string deviceName = "Nieznane urządzenie";

                try
                {
                    var hostEntry = await Dns.GetHostEntryAsync(ip);
                    if (!string.IsNullOrEmpty(hostEntry.HostName))
                    {
                        deviceName = hostEntry.HostName.Split('.')[0];
                    }
                }
                catch
                {
                    
                }

                return new NetworkDevice 
                {
                    IpAddress = ip,
                    Name = deviceName,
                    IsOnline = true
                };
            }
        } 
        catch 
        { 
        
        }
    
        return null;
    }
}