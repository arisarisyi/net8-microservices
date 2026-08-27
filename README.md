# Mango 🥭

A microservices playground built on **.NET 8**, modeled after a typical
e-commerce platform (Mango Shopping). The goal is to grow it step by step into
a complete distributed system — independent services, an API gateway, and a
client application — while practicing the patterns that hold them together.

> **Status:** early development. One backend service is live; everything else
> on the roadmap below is still ahead.

## Solution layout

Solution folders mirror the target architecture, so new projects land where
they belong:

| Folder        | Contents                                                                |
| ------------- | ----------------------------------------------------------------------- |
| `FrontEnd`    | `Mango.Web` — ASP.NET Core MVC client (template shell for now)          |
| `Services`    | `Mango.Services.CouponAPI` — discount coupon REST API                   |
| `Gateway`     | _(placeholder)_ future API gateway (e.g. Ocelot) fronting the services  |
| `Integration` | _(placeholder)_ message bus / shared infrastructure                     |

## Current services

### CouponAPI

Minimal Web API managing coupons in SQL Server via Entity Framework Core:

| Method & route                  | Purpose                          |
| ------------------------------- | -------------------------------- |
| `GET api/CouponAPI`             | List all coupons                 |
| `GET api/CouponAPI/{id:int}`    | Get one coupon by id             |
| `GET api/CouponAPI/GetByCode/{code}` | Get coupon by code (case-insensitive) |
| `POST api/CouponAPI`            | Create                           |
| `PUT api/CouponAPI`             | Update                           |
| `DELETE api/CouponAPI/{id:int}` | Delete                           |

Every action answers with the same envelope instead of raw values:

```json
{ "isSuccess": true, "result": { ... }, "message": "Success" }
```

Entity ↔ DTO mapping goes through AutoMapper. Swagger UI is enabled in
development at `/swagger`. Two coupons are seeded by migration so a fresh
database starts usable.

## Prerequisites

- [.NET SDK 8.x](https://dotnet.microsoft.com/download/dotnet/8.0) — pinned
  via `global.json`
- [Docker](https://www.docker.com/) — runs SQL Server locally
- `dotnet-ef` CLI tool (`dotnet tool install --global dotnet-ef`) — for migrations

## Getting started

1. **Start SQL Server** (SQL Server 2022 in Docker, data persisted in a named
   volume):

   ```bash
   docker compose up -d
   ```

   On Apple Silicon this pulls the amd64 image and runs it under Rosetta,
   since Microsoft ships no arm64 build. The container reports healthy once
   its healthcheck accepts logins.

2. **Create the database** from the checked-in migration:

   ```bash
   dotnet ef database update --project Mango.Services.CouponAPI
   ```

3. **Run the apps** (from Rider/Visual Studio, or per project):

   ```bash
   dotnet run --project Mango.Services.CouponAPI   # Swagger: http://localhost:5029/swagger
   dotnet run --project Mango.Web                  # Home:     http://localhost:5155
   ```

## Configuration

| Setting                        | Where                                  | Value                        |
| ------------------------------ | -------------------------------------- | ---------------------------- |
| SQL Server connection string   | `Mango.Services.CouponAPI/appsettings.json` | `Server=localhost,1433;Database=MangoCoupon;...` |
| sa password                    | `docker-compose.yml`                   | developer-only placeholder   |

The credentials here are strictly local-development values; replace them via
environment variables or user secrets before anything shared or deployed.

## Roadmap

- [ ] Consume CouponAPI from Mango.Web
- [ ] Authentication service + JWT-based auth across services
- [ ] API gateway (Ocelot) in the `Gateway` folder
- [ ] Message bus (e.g. RabbitMQ) integration for cross-service events
