using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSharpApi.Data;
using CSharpApi.Models;

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
}