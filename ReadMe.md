# Event Management System

Event management system is a web application for managing events, user registration for events, their reviews and ratings on events. It is implemented in modern technologies such as **Angular v21** (Signals, RxJs,SignalStore) for client side, **.NET 10** (Clean Architecture, CQRS Design Pattern, MediatR) for server side and **SQL Server** as a persistant database.

# Technologies used

## Frontend

- Angular (Standalone Components, Signals)
- NgRx SignalStore (State Management)
- Angular Materials (UI Components)
- Bootstrap (Grid system and spacing)

## Backend

- ASP.NET Core WebApi (WebApi Application)
- Class Libraries (Clean Architectur: Infrastructure, Domain, Application)
- EF Core (SQL Server ORM Tool)
- MediatR (CQRS Pattern)
- JWT (Bearer token)

# Installation and Running

## Backend

1. Enter root server folder 'event-management-server'.
2. Right click on webapi project folder, open Manage User Secrets add DefaultConnection for ConnectionStrings ex: "Server=(localdb)\\mssqllocaldb;Database=your_db_name;Trusted_Connection=True;MultipleActiveResultSets=true", add key for jwt longer than 32 characters.
3. Apply migrations, dotnet ef database update --project Infrastructure --startup-project event-management-server.

## Frontend

1. Enter root client folder 'event-management-server'
2. run npm install
3. Run application with command ng serve

# Key functionalities

- Authentification
- Event Listing
- New Events Creation, Deleting and Updating Your Own Events
- Filtering Events by Different Parameters
- Responsive Design
