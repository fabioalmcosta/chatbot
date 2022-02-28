# ChatBot

This is a chatbot created as a exmaple system with .NET Core / React JS / Postgres DB.

## Installation

The system is inside a Docker Compose. First you will need to have Docker and Docker Compose on your machine.
Then run the following command on the root folder

```bash
docker-compose up
```

## Migration

The command above should be enough to Build and start all the containers. But we need run the migrations and create the necessary tables in the database. For that you will ned to build the project locally and then run the following command on the folder "Migration" of your project.


```bash
dotnet fm migrate -p postgres -c "User ID=postgres;Password=password;Host=localhost;Port=5433;Database=ChatBot;" -a ".\bin\Debug\net5.0\Migration.dll"
```

maybe you will get an error on the above command if you do not have the dotnet fm migration tool. to install that run the following command then the migrations again
```bash
dotnet tool install -g FluentMigrator.DotNet.Cli
```

## Launching
after that the project will be ready to access.
the public links are:

## Frontend
```url
http://localhost:3000
```

## Backend
```url
http://localhost:5000
```

## Swagger api
```url
http://localhost:5000/swagger/index.html
```

## Postgres Database access
```url
http://localhost:5433
```

## License
[MIT](https://choosealicense.com/licenses/mit/)