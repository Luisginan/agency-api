# Introduction 
TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project. 

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
* Installation process
* Software dependencies
    ### Install Docker
     - [Install Docker on Windows][dockerInstallationGuide]
    ### Install Redis Postgres in Docker
       docker-compose up -d
    ### Create Table
       execute file Table.sql in Postgres 
    ### Swagger documentation (run the project and open the link in browser)
       dotnet run
       
* Latest releases
* API references

# Build and Test
  - dotnet build
  - dotnet test --filter "FullyQualifiedName!~IntegrationTest" --collect "XPlat Code Coverage"
  - Goto folder BlueprintTest\TestResults\{guid}\coverage.cobertura.xml
  - open terminal : dotnet tool install -g dotnet-reportgenerator-globaltool
  - open terminal : reportgenerator -reports:coverage.cobertura.xml -reporttypes:html -targetDir:.
  - open file index.html in browser
