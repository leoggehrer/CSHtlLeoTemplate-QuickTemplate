# Kommandos

## DotNet Kommandos

### Vorraussetzung

Damit die Kommandos fuer die Datenbank-Erstellung (Migration und Update) ausgefuehrt werden, muss das EF-Tool installiert sein. Dieses Tool wird mit folgendem Befehl installiert:

```code
dotnet tool install --global dotnet-ef
```

### Create migration (database)

#### Migration fuer die Logik erstellen

```code
dotnet ef migrations add InitDb --startup-project QuickTemplate.ConApp --project QuickTemplate.AppLogic
```

### Update database mit den AppSettings aus der Konsolen-App erstrellen

```code
dotnet ef database update --startup-project QuickTemplate.ConApp --project QuickTemplate.AppLogic
```

#### Migration fuer den UnitTest erstellen

```code
dotnet ef migrations add InitDb --startup-project QuickTemplate.Logic.UnitTest --project QuickTemplate.AppLogic
```

### Update database mit den AppSettings aus dem UnitTest erstrellen

```code
dotnet ef database update --startup-project QuickTemplate.Logic.UnitTest --project QuickTemplate.AppLogic
```

## Docker Kommandos

### MSSQL-Server auf dem Mac installieren

```code
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=1234_Passme" -p 1433:1433 --name mssqltest --volume=/Users/ggehrer/Projects/Data/docker/mssqltest:/var/opt/mssql/ -d mcr.microsoft.com/azure-sql-edge:latest
```

## ML.NET Kommands

### Installation der ML.NET CLI auf dem MAC

```code
dotnet tool install --global mlnet-osx-arm64
```

### Starten der ML.NET CLI mit der Auflistung der Hilfe àm MAC

```code
~/.dotnet/tools/mlnet --help [command]
```
