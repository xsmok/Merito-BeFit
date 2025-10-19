# BeFit

A simple fitness tracking web application built with ASP.NET Core MVC that helps users manage their exercises, training sessions, and workouts.

## Table of Contents

- [Project Description](#project-description)
- [Tech Stack](#tech-stack)
- [Getting Started Locally](#getting-started-locally)
- [Available Scripts](#available-scripts)
- [Project Scope](#project-scope)
- [Project Status](#project-status)
- [License](#license)

## Project Description

BeFit is a straightforward fitness tracking application designed to help users organize their workout routines. The application follows a simple three-tier data model:

- **Exercise**: A basic entity representing a specific exercise with just a name (e.g., "Bench Press", "Squats")
- **Training Session**: A time-bounded entity that tracks when you worked out, with start and end timestamps
- **Workout**: The connection between an exercise and a training session, containing the actual performance data such as series, repetitions, and weight

This architecture allows users to plan their exercises, schedule training sessions, and record detailed workout performance data all in one place.

## Tech Stack

### Backend
- **.NET 8.0** - Latest version of the .NET platform
- **ASP.NET Core MVC** - Web framework for building server-side rendered applications
- **Entity Framework Core 9.0** - Object-relational mapper (ORM) for database operations
- **SQLite** - Lightweight, file-based database for data persistence
- **ASP.NET Core Identity** - Authentication and authorization framework

### Frontend
- **Bootstrap 5** - Responsive CSS framework
- **jQuery** - JavaScript library for DOM manipulation
- **jQuery Validation** - Client-side form validation
- **jQuery Validation Unobtrusive** - Seamless integration with ASP.NET Core validation

### Development Tools
- **Entity Framework Core Tools** - Command-line tools for migrations and database management
- **ASP.NET Core Diagnostics for EF Core** - Development diagnostics for Entity Framework

## Getting Started Locally

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed on your machine
- A code editor (Visual Studio 2022, Visual Studio Code, or JetBrains Rider recommended)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd BeFit
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Apply database migrations**
   ```bash
   dotnet ef database update --project BeFit/BeFit.csproj
   ```

   This will create the SQLite database file with all necessary tables.

4. **Run the application**
   ```bash
   dotnet run --project BeFit/BeFit.csproj
   ```

5. **Access the application**

   The application will be available at:
   - HTTPS: https://localhost:7171
   - HTTP: http://localhost:5155

## Available Scripts

### Building and Running

```bash
# Build the entire solution
dotnet build

# Build specific project
dotnet build BeFit/BeFit.csproj

# Run the application (development mode with HTTPS)
dotnet run --project BeFit/BeFit.csproj

# Run with specific launch profile
dotnet run --project BeFit/BeFit.csproj --launch-profile https
dotnet run --project BeFit/BeFit.csproj --launch-profile http

# Clean build artifacts
dotnet clean

# Restore NuGet packages
dotnet restore
```

### Database Management

```bash
# Create a new migration
dotnet ef migrations add <MigrationName> --project BeFit/BeFit.csproj

# Update database to latest migration
dotnet ef database update --project BeFit/BeFit.csproj

# Remove last migration (if not applied to database)
dotnet ef migrations remove --project BeFit/BeFit.csproj
```

### Launch Profiles

The application includes three launch profiles:

- **https** (default): Runs on https://localhost:7171 and http://localhost:5155
- **http**: Runs on http://localhost:5155 only
- **IIS Express**: Uses IIS Express with URL http://localhost:4709 (SSL port: 44316)

## Project Scope

### Core Features

The BeFit application is designed to support the following functionality:

1. **Exercise Management**
   - Create, read, update, and delete exercises
   - Store exercise definitions with descriptive names

2. **Training Session Tracking**
   - Schedule and track training sessions
   - Record start and end times for each session
   - View training history

3. **Workout Recording**
   - Link exercises to training sessions
   - Record performance metrics:
     - Number of series (sets)
     - Repetitions per series
     - Weight used
   - Track progress over time

### Current Implementation

The project currently includes:
- ASP.NET Core Identity setup with user authentication
- SQLite database configuration
- MVC architecture with Controllers, Models, and Views
- Bootstrap 5 responsive layout
- Role-based authorization foundation (Adult role)

### Architecture Overview

```
BeFit/
├── Controllers/     # MVC controllers for handling HTTP requests
├── Models/          # Data models and view models
├── Data/            # Database context and configurations
├── Views/           # Razor views (.cshtml files)
│   ├── Home/        # Home page views
│   ├── Shared/      # Shared layouts and partials
│   └── ...
├── wwwroot/         # Static files (CSS, JS, images)
│   ├── css/
│   ├── js/
│   └── lib/         # Frontend libraries
└── Program.cs       # Application entry point
```

## Project Status

**Current Status**: 🚧 In Active Development

### Completed
- ✅ ASP.NET Core MVC project setup
- ✅ Entity Framework Core integration with SQLite
- ✅ ASP.NET Core Identity authentication system
- ✅ Bootstrap 5 responsive layout
- ✅ Basic MVC structure (Controllers, Models, Views)

### In Progress
- 🔄 Exercise management features
- 🔄 Training session tracking
- 🔄 Workout recording and linking

## Development Guidelines

This project follows beginner-friendly development practices:

- **Clear Code**: Uses descriptive variable names and comments for complex logic
- **Defensive Coding**: Includes input validation and error handling
- **Conventional Commits**: Follows the format `type(scope): description`
  - Types: `feat`, `fix`, `docs`, `style`, `refactor`, `test`, `chore`
  - Example: `feat(exercise): add create exercise form`

For detailed development guidelines, please refer to [CLAUDE.md](CLAUDE.md).

## Contributing

Contributions are welcome! Please ensure you:

1. Follow the conventional commit format
2. Write clear, well-commented code
3. Include appropriate error handling
4. Test your changes thoroughly

## License

This project does not currently specify a license. Please contact the project maintainers for licensing information.

## Support

For questions, issues, or feature requests, please open an issue in the GitHub repository.

---

**Note**: This is a learning project following beginner-friendly development practices. The codebase prioritizes clarity and educational value over advanced optimization techniques.
