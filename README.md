## Tech Challenge - FIAP Cloud Games

Pós Tech em Arquitetura em Sistemas .NET.

## Microsserviço de Pagamentos/Compras do Usuário

Resolvi usar as seguintes dependências:
```bash
# No Infrastructure (EF Core + PostgreSQL)
dotnet add FIAPCloudGames.Infrastructure package Microsoft.EntityFrameworkCore
dotnet add FIAPCloudGames.Infrastructure package Microsoft.EntityFrameworkCore.Design
dotnet add FIAPCloudGames.Infrastructure package Microsoft.EntityFrameworkCore.Tools
dotnet add FIAPCloudGames.Infrastructure package Microsoft.EntityFrameworkCore.Tools
dotnet add FIAPCloudGames.Infrastructure package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add FIAPCloudGames.Infrastructure package LiteDB

# No API (Swagger e JWT)
dotnet add FIAPCloudGames.API package Swashbuckle.AspNetCore
dotnet add FIAPCloudGames.API package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add FIAPCloudGames.API package Microsoft.EntityFrameworkCore.Design
```  

Já que estou usando uma solução com vários projetos, o comando para rodar migrations é um pouco "diferente":
```bash
dotnet ef migrations add NomeDaMigration --project FIAPCloudGames.Infrastructure --startup-project FIAPCloudGames.API
dotnet ef database update --project FIAPCloudGames.Infrastructure --startup-project FIAPCloudGames.API
```
</details>


