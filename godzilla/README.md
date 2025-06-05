# Godzilla API

### Como executar com docker?

Uma vez feito clone desse repositório:

```bash
    git clone https://github.com/misesdev/a5-solutions.git
```

Navegue até a pasta do projeto:

```bash
    cd a5-solutions/godzilla/api/
```

Primeiro instale os pacotes:

```bash
    dotnet restore
```

O projeto está configurado para executar com docker compose, porém existe um detalhe, o banco de 
dados acaba não conectando, para garantir que não hajam problemas, primeiro execute a aplicação
com docker compose:

```bash
    docker compose up --build -d
```

Uma vez executando dentro do container, a aplicação já estará disponível em `http://localhost:8080`, 
porém, ainda falta executar as migrations para funcionar corretamente. Altere a conectionString no arquivo `appsettings.Development`,
apenas o host deve ser alterado para localhost:

```json 
    {
        "ConnectionStrings": {
            "DefaultConnection": "Server=sqlserver,1433;Database=godzilla;User Id=sa;Password=pass@WdHash500;Encrypt=False;TrustServerCertificate=True;",
        }
    }  
    // alterado:
        {
        "ConnectionStrings": {
            "DefaultConnection": "Server=localhost,1433;Database=godzilla;User Id=sa;Password=pass@WdHash500;Encrypt=False;TrustServerCertificate=True;",
        }
    }
```

Agora basta executar as migrations e a aplicação deve funcionar como o esperado:

```bash
    dotnet ef database update 
```

### Executar apenas o banco de dados com docker

Se você estiver em linux basta deletar tudo que está em `docker-compose.yml` e substituir por:

```
    services:
      sqlserver:
        image: mcr.microsoft.com/mssql/server:2022-latest
        container_name: sqlserver
        environment:
          - ACCEPT_EULA=Y
          - MSSQL_SA_PASSWORD=pass@WdHash500
          - MSSQL_PID=Express
    network_mode: host
```

Então executando com docker compose irá executar apenas o banco de dados em um container que ficará 
disponível em `http://localhost:1433`, então altere apenas o host na connectionString e então 
você pode executar a apliação normalmente:

```json 
    {
        "ConnectionStrings": {
            "DefaultConnection": "Server=sqlserver,1433;Database=godzilla;User Id=sa;Password=pass@WdHash500;Encrypt=False;TrustServerCertificate=True;",
        }
    }  
    // alterado:
        {
        "ConnectionStrings": {
            "DefaultConnection": "Server=localhost,1433;Database=godzilla;User Id=sa;Password=pass@WdHash500;Encrypt=False;TrustServerCertificate=True;",
        }
    }
```

```bash
    dotnet run
```

### Como executar sem docker no Windows

Para executar a aplicação no Windows, basta mudar a connectionString para apontar para seu banco de dados
rodando localmente, executar as migrations:

```bash
    dotnet ef database update 
```

E então executar uma instância de desenvolvimento:

```bash
    dotnet run
```
