# Introduction 
This repository contains:
1. The entity framework based management of the core relational entities used by Fixit
2. The CI/CD terraform-based files, and Azure yaml, allowing the deployment of this service
3. The RestAPI interface (deployed in a multi-tenant fashion), allowing for the client application to get relevant data 

# Getting Started
1.	This repository contains an entity-framework based project (MSSSQL), which can be initialized through the program.cs file
2.	It also contains the rest api, of which the entry point is also the program.cs file of the relevant project
3.	There are internal nuget packages that this service uses, of which all can be found under the Fixit "list"
4.	Official releases of this service are held in a private Azure Devops project, and will be migrated here shortly

# Build and Test
1. For each C# project, there exists an equivalent unit tests project, all under the same solution. 
