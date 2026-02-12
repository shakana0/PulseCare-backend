## Pulse Care – Backend Engine
## Lead Developer & System Architect
Pulse Care is a digital healthcare platform. As the **Tech Lead**, I was responsible for the initial system design, infrastructure orchestration, and core backend services.

---

## 🏗 Key Technical Contributions
* **Infrastructure & DevOps:** Orchestrated the entire development environment using **Docker Compose** and managed the cloud transition from local containers to **Azure App Service** and **Azure SQL Database**.
* **Real-time Communication:** Architected the messaging backbone using **SignalR**, including a `CustomUserIdProvider` to ensure secure and accurate patient-doctor routing.
* **Data Architecture:** Designed domain models and implemented the **Repository Pattern** managed via **Entity Framework Core** migrations.
* **Security:** Implemented **JWT Bearer Authentication**, tailoring token validation via `appsettings.json` for complex claim structures.
* **Data Seeding:** Developed a robust seed data engine for a consistent testing environment across the team of 10 developers.

---

## ⚖️ Engineering Leadership & Quality Assurance
As the technical gatekeeper, I ensured code quality and architectural consistency:

* **Active Code Reviews:** Conducted **18+ rigorous code reviews** focusing on performance, security, and pattern adherence.
* **Mentorship:** Guided the team in implementing asynchronous operations and the Repository Pattern to minimize technical debt.
* **Policy Enforcement:** Established the **Database Schema Change Policy** below to prevent migration conflicts and environment drifts.

### 📈 Evidence of Leadership
![Code Review Evidence](image_5cf835.png)
*Snapshot from GitHub Insights showing 18 conducted reviews and key contributions.*

---

## 🛠 Tech Stack
* **Backend:** ASP.NET Core API (.NET 9)
* **Database:** SQL Server (Docker / Azure SQL)
* **Patterns:** Repository Pattern, Dependency Injection, Singleton Providers
* **Real-time:** SignalR
* **Cloud:** Azure (App Service, SQL DB)

---

## ⚠️ Database Schema Change Policy
*Required reading for backend contributors.*
To maintain database integrity, only the **Tech Lead** is authorized to modify the schema (Entities, DbContext, Migrations, Seed Data). 

---

# 🚀 Getting Started – PulseCare Backend
---
## 🐳 Start the SQL Server Database (Docker): 

1. Start the database:
```bash
docker compose up -d
```

2. Stop the database:
```bash
docker compose down
```
3. Apply EF Core Migrations:
```bash
dotnet ef database update
```

4. Start the API:
```bash
dotnet run
```
5. If you need to reset the database completely (removes all data):
```bash
docker compose down -v
```
---
## 🔒 Database & Migrations Policy

✅ Allowed for all developers:
```bash
dotnet ef database update
docker compose up -d
docker compose down -v
```

❌ Not allowed for other team members:
```bash
dotnet ef migrations add <Name>
dotnet ef migrations remove
dotnet ef database drop
```
## Why this policy exists
- Prevents migration conflicts
- Ensures consistent schema across all environments
- Avoids accidental data loss
- Keeps the backend stable and predictable
- Makes debugging significantly easier
