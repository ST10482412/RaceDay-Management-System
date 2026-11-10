# RaceDay Management System

## Project Overview

RaceDay Management System is a web-based application developed using ASP.NET Core Web API and ASP.NET Core MVC.
<img width="1366" height="768" alt="Screenshot (59)" src="https://github.com/user-attachments/assets/f5a7dd65-2724-48fa-9b0a-ddc2710eeae3" /> my docker didn't want to open apologies
The system is designed to support the managem
ent of racing events, organisers, participants and event categories.

The solution consists of two connected projects:

- **RaceDayAPI:** Handles backend operations, authentication, business logic and database communication.
- **RaceDayMVC:** Provides the user interface through which organisers and participants interact with the system.

The application demonstrates the integration of a RESTful API with an MVC web application.

## Technologies Used

- C#
- ASP.NET Core Web API (.NET 8)
- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- Microsoft SQL Server
- SQL Server Management Studio (SSMS)
- JWT Authentication
- Cookie Authentication
- Bootstrap
- HTML, CSS and Razor Views
- Swagger / OpenAPI
- Visual Studio 2022
- Git and GitHub

## Project Structure

```text
RaceDay-Management-System/
│
├── RaceDayAPI/
│   ├── Controllers/
│   ├── Models/
│   ├── Data/
│   ├── Program.cs
│   └── RaceDayAPI.csproj
│
├── RaceDayMVC/
│   ├── Controllers/
│   ├── Models/
│   ├── Views/
│   ├── Services/
│   ├── wwwroot/
│   ├── Program.cs
│   └── RaceDayMVC.csproj
│
├── RaceDay.sln
├── .gitignore
└── README.md
```

*The structure above is illustrative. Actual folders and the solution filename may differ.*

## System Functionality

### Authentication and Authorisation

The system includes functionality for:

- User registration and login.
- JWT-based authentication in the API.
- Cookie-based authentication in the MVC application.
- Role-based access for organisers and participants.
- Protected pages and API endpoints.

### Organiser Functionality

Organisers can access an organiser dashboard and manage racing information through the MVC application.

Implemented areas include:

- Event management.
- Event category management.
- Creating, viewing, editing and deleting records.
- Ownership checks to restrict access to relevant records.

### Participant Functionality

The application includes participant authentication and a participant dashboard.

### Database Integration

Entity Framework Core is used to communicate with Microsoft SQL Server.

The database model includes entities for:

- Users
- Organisers
- Participants
- Events
- Categories
- Enrolments
- Results

The presence of an entity in the database model does not necessarily mean all associated user-facing functionality is complete.

## Application Architecture

The application uses a layered client-server approach.

```text
User / Web Browser
        |
        v
RaceDayMVC
(Razor Views and Controllers)
        |
        v
RaceDayAPI
(RESTful API Endpoints)
        |
        v
Entity Framework Core
        |
        v
SQL Server Database
```

The MVC application sends HTTP requests to the API. The API processes requests, applies authentication and business rules, and communicates with the SQL Server database.

## How to Run the Project

### Prerequisites

Install:

1. Visual Studio 2022 with ASP.NET and web development support.
2. .NET 8 SDK.
3. Microsoft SQL Server.
4. SQL Server Management Studio.

### Setup Instructions

1. Clone or download this GitHub repository.
2. Open the solution file in Visual Studio.
3. Restore NuGet packages.
4. Configure the SQL Server connection string locally.
5. Configure a secure JWT signing key using local configuration or user secrets.
6. Apply Entity Framework Core migrations where required.
7. Configure RaceDayMVC to use the correct RaceDayAPI base URL.
8. Start RaceDayAPI.
9. Start RaceDayMVC.
10. Open the MVC application in a web browser.

**Security:** Do not commit database passwords, JWT signing keys or other sensitive credentials to GitHub.

## Testing and Verification

The application can be checked using:

- Visual Studio build and debugging tools.
- Swagger for testing API endpoints.
- SQL Server Management Studio for checking database records.
- Manual browser testing of MVC functionality.
- GitHub for version control and source-code management.

## Current Development Status

The project currently includes the API and MVC applications, authentication integration, organiser and participant dashboards, and event and category management functionality.

The following areas require further implementation or verification:

- Participant event enrolment workflows.
- Race results management.
- Containerisation and deployment.

## Future Improvements

Potential future improvements include:

- Complete participant event registration.
- Race results recording and display.
- Additional validation and automated tests.
- Improved dashboards and reporting.
- Deployment to a hosting environment.

## Project Purpose

This project was developed as part of an academic software development assignment to demonstrate practical skills in C#, ASP.NET Core, database integration, authentication, web development and version control.

## Author

Student project – RaceDay Management System.
