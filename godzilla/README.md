# Godzilla API

## Teste #1

**1 - Você deve disponibilizar um endpoint "/godzilla". Esse serviço permite o aluguel de um filme 
somente se a locadora possuir este filme em estoque. Caso o aluguel do filme escolhido seja 
permitido, você deve retornar HTTP 200-OK, caso contrário um HTTP 403-FORBIDDEN:**

Para atender esse caso eu foi implementado um endpoint que recebe o id do filme, como implementei
tudo em inglês, então ficou `/movies/rent/{id}` que retornara a resposta como desejado, caso tenha
em estoque para alugar, retorna `HTTP 200 OK` e caso não esteja `HTTP $)# FORBIDDEN`.


## Teste #2

**2 - Disponibilizar um endpoint “localdora/godzilla” um HTTP GET que nos retorne os dados referente ao
filme especificado. A pesquisa deve retornar o filme que contenham o parâmetro informado no título, 
não é necessário informar o título inteiro do filme, assim sendo, pode retornar mais de um filme:**

Para atender esse caso, novamente, como implementei em inglês a aplicação, criei 3 endpoints:

- `/movies/new`
    Permite adicionar filmes ao banco de dados para que possam ser utilizados os outros endpois.
- `/movies/movie/{id}`
    Retorna os dados do filme ao qual id se refere.
- `/movies/search`
    Retorna uma lista paginada com os filmes aos quais o título contem o termo pesquisado. 
    recebe como parâmetro o json:
    ```json
        {
            searchTerm: string,
            page: number,
            itemsPerPage: number
        }
    ```

    Retorna um json com a estrutura:

    ```json
        {
            page: number,
            itemsPerPage: number,
            items: [{}...]
        }
    ```

## Test #3

**3 - Disponibilize um serviço extra de usuários, no qual permita incluir um novo cliente “usuários/usuário ” 
que responde um HTTP POST e ao realizar a autenticação do usuário, deve retornar um o Token para 
ser utilizado nas próximas requisições:**

Para atender o caso acima a authenticação foi implementada com Jwt(Json Web Token), foram criados
2 endpoints simples:

- `/auth/signup`
    Permite o cadastro de um usuário, recebendo como parâmetro o json: 
    ```json
        {
          "name": "string",
          "email": "user@example.com",
          "password": "string"
        }
    ```
- `/auth/sign`
    Permite o login de um usuário retornando um token ao qual será utilizado para authenticar o 
    usuário para o restante dos endpoints.
    Recebe como parametro o json:
    ```json
        {
          "email": "user@example.com",
          "password": "string"
        }
    ```
    Retorna o json:
    ```json
        {
            {
                "auth": true,
                "user": {
                    "name": "string",
                    "email": "user@example.com",
                    "id": "120e15b1-f80d-44ee-b368-3e8be0a5616c",
                    "createdAt": "2025-06-05T10:27:47.6244214",
                    "updatedAt": null,
                    "deletedAt": null
                },
                "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEyMGUxNWIxLWY4MGQtNDRlZS1iMzY4LTNlOGJlMGE1NjE2YyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InVzZXJAZXhhbXBsZS5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic3RyaW5nIiwiZXhwIjoxNzQ5MTIyOTA0LCJpc3MiOiJHb2R6aWxsYUFwaSIsImF1ZCI6IkdvZHppbGxhVXNlcnMifQ.3no5ErBXccDLvG3uRdSD9sQYWaUlK4Nc86psA8dcSJs"
            }
        }
    ```


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

Uma vez executando dentro do container, a aplicação já estará disponível em `http://localhost:8080/swagger`, 
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

## Executar os testes

Foram implementados diferentes tipos de testes automatizados e devem ser executados 
independentemente por categoria:
    
- `Testes de Unidade` - Testa componentes independentes da aplicação. Devem ser executado
filtrando pela Categoria `Unit`;

- `Testes de Integração` - Testa as classes que interagem diretamente com o banco de dados,
criando cenários com (select, update, delete). Devem ser executados filtrando pela 
categoria `Integration`;

- `Testes de Validação` - Testa a validação das models, efetivamente as validações feitas nos 
parâmetros recebidos nas controllers. Devem ser executados filtrando pela categoria `Validation`.

- `Testes de API` - Testa a API executando requests para cada endpoint em cenários desenhados, 
e devido a essa naturesa, para executá-los corretamente primeiro exeute a aplicação, uma 
vez acessível em `http://localhost:8080`, então os testes podem ser executados filtrando
pela categoria `Controller`.


Para executas os testes então basta navegar até a pasta do projeto de testes, e executar o comando 
abaixo (Lembre-se de trocar `{Categoria}`, pela categoria de testes detalhadas acima):

```bash 
    cd a5-solutions/godzilla/tests/ &&
    dotnet test --filter "TestCategory={Categoria}" --logger "console;verbosity=detailed"
```


`Obs`

O projeto foi feito em um ubuntu utilizando `Lunar Vim` sem uma IDE, então possívelmente o projeto
não vai abrir certinho com o projeto de tests etc em uma IDE como Visual Studio, não tem 
arquivos de solutions(*.sln), por isso foi utilizando docker para poder facilitar a execução em 
Windows sem muitos problemas.
