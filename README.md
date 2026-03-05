# APS Ahilyanagar Alumni Engagement Portal

A full-stack alumni portal built for Army Public School (APS) Ahilyanagar, enabling students and alumni to browse, search, and connect with the school's alumni network.
Created using the tech stack asked by the stakeholders.

## Tech Stack

| Layer    | Technology                                      |
| -------- | ----------------------------------------------- |
| Backend  | ASP.NET Core (.NET 9), C#                       |
| ORM      | Entity Framework Core (Pomelo MySQL provider)   |
| Database | MySQL 8.0                                       |
| Frontend | HTML5, CSS3, Vanilla JavaScript, Font Awesome   |
| Hosting  | Render (Docker-based deployment)                |

## Features

- **Alumni Search & Filtering** - Search alumni by keyword, batch year, organization, domain/category, and achievement via a REST API.
- **Notable Alumni Showcase** - Homepage highlights notable alumni with animated card reveals on scroll.
- **Responsive Design** - Mobile-friendly layout with a shrinking header on scroll and smooth navigation.
- **Google Maps Integration** - Embedded map in the footer showing the school location.
- **Alumni Registration** - Links to a Google Form for new alumni to register and join the network.
- **Health Check Endpoint** - `/health` endpoint for monitoring database connectivity and app status.
- **Dockerized Deployment** - Multi-stage Dockerfile for optimized production builds on Render.

## Project Structure

```
MyAlumniApp/
├── Controllers/
│   └── PeopleController.cs      # Search API endpoint
├── Data/
│   └── AppDbContext.cs           # EF Core database context
├── Models/
│   └── Person.cs                 # Alumni data model
├── wwwroot/
│   ├── index.html                # Homepage with search, filters, notable alumni
│   ├── results.html              # Search results page
│   ├── design.css                # Homepage styles
│   ├── style.css                 # Results page styles
│   ├── script.js                 # Scroll animations, smooth navigation
│   └── images/                   # Alumni photos
├── Program.cs                    # App configuration, middleware, startup
├── Dockerfile                    # Multi-stage Docker build
├── render.yaml                   # Render deployment config
└── appsettings.json              # App configuration
```

## API

### `GET /api/people/search`

Search and filter alumni profiles.

| Parameter      | Type   | Description                        |
| -------------- | ------ | ---------------------------------- |
| `keyword`      | string | Search by name, description, domain |
| `year`         | string | Filter by batch year               |
| `organization` | string | Filter by organization             |
| `achievement`  | string | Filter by award/achievement        |
| `category`     | string | Filter by domain/category          |

### `GET /health`

Returns application and database health status.

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- MySQL 8.0+

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/<your-username>/MyAlumniApp.git
   cd MyAlumniApp
   ```

2. **Configure the database**

   Set the following environment variables (or update `appsettings.json`):
   ```
   MYSQLHOST=localhost
   MYSQLPORT=3306
   MYSQLDATABASE=aps_alumni
   MYSQLUSER=root
   MYSQLPASSWORD=yourpassword
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```
   The app will be available at `http://localhost:5000` (or the port shown in the console).

### Docker

```bash
docker build -t myalumniapp .
docker run -p 8080:8080 \
  -e MYSQLHOST=host.docker.internal \
  -e MYSQLDATABASE=aps_alumni \
  -e MYSQLUSER=root \
  -e MYSQLPASSWORD=yourpassword \
  myalumniapp
```

## Deployment

The app is configured for deployment on [Render](https://render.com/) using the included `render.yaml` and `Dockerfile`.

## License

This project was built for APS Ahilyanagar Alumni Association.
