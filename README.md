# Network Device Manager (API & Discovery Tool)

A lightweight, full-stack web application designed to discover, track, and manage devices on a local network. Built as a practical demonstration of backend development with **.NET 10**, asynchronous programming, and RESTful API architecture.

## Key Features

* **Network Discovery (Ping Sweep):** Features an asynchronous network scanner that sweeps the local subnet (e.g., `192.168.0.x`) using ICMP Ping packets to detect active devices.
* **Reverse DNS Lookup:** Automatically attempts to resolve hostnames for discovered IP addresses using `System.Net.Dns`.
* **RESTful CRUD Operations:** Fully implemented endpoints (GET, POST, PUT, DELETE) to manually manage the network inventory.
* **Entity Framework Core ORM:** Uses SQLite for lightweight, reliable data persistence with automated migrations.
* **Modern Vanilla UI:** A clean, responsive frontend built with HTML, pure JavaScript (Fetch API), and Pico.css, served directly from the .NET Web API `wwwroot`.

## Tech Stack

* **Backend:** C# / ASP.NET Core 10 Web API
* **Database:** SQLite & Entity Framework Core (EF Core)
* **Frontend:** HTML5, JavaScript (ES6+), Pico.css
* **Networking Tools:** `System.Net.NetworkInformation.Ping`, `Task.WhenAll` (Concurrency)

## How to run locally

1. Ensure you have the [.NET 10 SDK](https://dotnet.microsoft.com/download) installed.
2. Clone this repository to your local machine.
3. Open a terminal in the project directory.
4. Run the database migrations (if not already applied):
   ```bash
   dotnet ef database update
   ```
5. Start the application:
   ```bash
   dotnet run
   ```
6. Open your browser and navigate to `http://localhost:<port>` to view the UI, or `http://localhost:<port>/swagger` to test the API directly.
