# ⚠️ Database Schema Change Policy (Read Before Working on Backend)
Only the backend maintainer is allowed to modify the database schema.  
This includes editing: 
- Entity classes
- DbContext
- Migrations
- Seed data structure.

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
