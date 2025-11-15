# AI Rules for BeFit

BeFit is a very simple fitness app. Where user can declare exercise types and training sessions (called "exercises" in the code), and link them together through exercise sessions.

- **ExerciseType**: Simple entity with just a name (e.g., "Bench Press", "Squats")
- **Exercise**: Time-bounded training session entity with start/end timestamps
- **ExerciseSession**: The connection between an ExerciseType and an Exercise (training session)
  - Note: Performance data (series, reps, weight) is planned but not yet implemented in the current model

## CODING_PRACTICES

### Guidelines for SUPPORT_LEVEL

#### SUPPORT_BEGINNER

- When running in agent mode, execute up to 3 actions at a time and ask for approval or course correction afterwards.
- Write code with clear variable names and include explanatory comments for non-obvious logic. Avoid shorthand syntax and complex patterns.
- Provide full implementations rather than partial snippets. Include import statements, required dependencies, and initialization code.
- Add defensive coding patterns and clear error handling. Include validation for user inputs and explicit type checking.
- Suggest simpler solutions first, then offer more optimized versions with explanations of the trade-offs.
- Briefly explain why certain approaches are used and link to relevant documentation or learning resources.
- When suggesting fixes for errors, explain the root cause and how the solution addresses it to build understanding. Ask for confirmation before proceeding.
- Offer introducing basic test cases that demonstrate how the code works and common edge cases to consider.


### Guidelines for VERSION_CONTROL

#### CONVENTIONAL_COMMITS

- Follow the format: type(scope): description for all commit messages
- Use consistent types (feat, fix, docs, style, refactor, test, chore) across the project
- Define clear scopes based on {{project_modules}} to indicate affected areas
- Include issue references in commit messages to link changes to requirements
- Use breaking change footer (!: or BREAKING CHANGE:) to clearly mark incompatible changes
- Configure commitlint to automatically enforce conventional commit format

## Development Commands

### Building and Running
```bash
# Build the solution
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

### Launch Profiles
- **https**: Runs on https://localhost:7171 and http://localhost:5155 (default)
- **http**: Runs on http://localhost:5155 only
- **IIS Express**: Uses IIS Express with URL http://localhost:4709 (SSL port: 44316)

## Project Architecture

### Directory Structure
```
BeFit/
├── Controllers/     # MVC controllers - handles HTTP requests and responses
├── Models/          # Data models and view models
├── Views/           # Razor views (.cshtml files)
│   ├── Home/        # Views for HomeController
│   ├── Shared/      # Shared views (_Layout.cshtml, Error.cshtml, etc.)
│   ├── _ViewImports.cshtml  # Global using directives for views
│   └── _ViewStart.cshtml    # Default layout configuration
├── wwwroot/         # Static files (CSS, JS, images)
│   ├── css/
│   ├── js/
│   └── lib/         # Client-side libraries (Bootstrap, jQuery)
├── Properties/      # Launch settings and configuration
├── Program.cs       # Application entry point and middleware configuration
└── appsettings.json # Application configuration
```

### MVC Pattern Implementation

**Program.cs** configures the application pipeline:
- Services are registered with `builder.Services.AddControllersWithViews()`
- Middleware pipeline includes: HTTPS redirection, static files, routing, authorization
- Default route pattern: `{controller=Home}/{action=Index}/{id?}`
- Error handling differs between Development and Production environments

**Controllers** (namespace: `BeFit.Controllers`):
- Inherit from `Controller` base class
- Use dependency injection (e.g., `ILogger<T>`)
- Return `IActionResult` for view rendering
- Currently contains only `HomeController` with Index, Privacy, and Error actions

**Models** (namespace: `BeFit.Models`):
- Currently contains only `ErrorViewModel` for error page rendering
- New models should be added here

**Views**:
- Use Razor syntax (.cshtml)
- `_Layout.cshtml` defines the common page structure
- `_ViewImports.cshtml` contains global using directives and tag helpers
- `_ViewStart.cshtml` sets the default layout for all views

### Configuration

**appsettings.json** / **appsettings.Development.json**:
- Logging levels configured (Default: Information, Microsoft.AspNetCore: Warning)
- Development and Production settings are separated
- AllowedHosts set to "*"

### Frontend Dependencies

The application uses:
- **Bootstrap 5** - CSS framework for responsive design
- **jQuery** - JavaScript library
- **jQuery Validation** - Client-side form validation
- **jQuery Validation Unobtrusive** - Unobtrusive validation support for ASP.NET Core

All frontend libraries are located in `wwwroot/lib/`.

## Project Configuration

- **Target Framework**: .NET 8.0
- **Nullable Reference Types**: Enabled
- **Implicit Usings**: Enabled
- **SDK**: Microsoft.NET.Sdk.Web

## Adding New Features

### Adding a Controller
1. Create new file in `Controllers/` directory
2. Use namespace `BeFit.Controllers`
3. Inherit from `Controller`
4. Add corresponding views in `Views/{ControllerName}/`

### Adding a Model
1. Create new file in `Models/` directory
2. Use namespace `BeFit.Models`

### Adding a View
1. Create `.cshtml` file in appropriate `Views/` subdirectory
2. Use `@model` directive if binding to a model
3. Views automatically have access to directives from `_ViewImports.cshtml`

## Current Limitations

- No database or Entity Framework Core configured yet
- No authentication/authorization implemented
- No unit tests or integration tests in solution
- No API controllers (only MVC controllers for server-side rendering)
