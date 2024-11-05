# Be Sport Smart - Backend

The backend for Be Sport Smart follows a **microservice-monorepo** approach, built with **.NET 8**. This architecture allows shared code to reside in a single repository while maintaining logical separation between modules. Each module is designed as a bounded context, containing only the necessary configurations and registrations specific to its functionality.

## Architecture

-   **Microservice-Monorepo**: All modules are in one repository for easy sharing of code and resources, but they are logically separated for modular development.
-   **Reversed Dependency Pattern**: The **DAL** layer references core modules that describe necessary interfaces. The actual implementation of these interfaces resides in the DAL layer, ensuring dependency inversion and separation of concerns.
-   **Bounded Modules**: Each module, such as the Identity module, has its own startup configuration file. This file registers services, commands, and one-time jobs specific to the module (e.g., sign-in/sign-up commands, default user provisioning, etc.).
-   **Cache-First Approach**: The system relies heavily on caching to optimize performance. Most logic and calculations are fetched from cache, as updates are infrequent. When an admin updates records in the database, it triggers an event that prompts the infrastructure to run a job syncing the changes back to the cache.

## Project Structure

Here is an overview of the main folders and files in the project:

	`bootstraps/
	│   ├── Bss.Bootstrap/             # Main entry point for bootstrapping modules as a service
	│       ├── Program.cs             # Program entry for the Bootstrap project
	│       ├── Bss.Bootstrap.csproj   # Bootstrap project file
	src/
	│   ├── Bss.Component.Identity     # Identity module, handling user authentication and registration
	│   ├── Bss.Core.Admin             # Admin module core functionalities
	│   ├── Bss.Core.BL                # Business Logic core functionalities
	│   ├── Bss.Core.Engine            # Core Engine responsible for calculations across implemented language engines
	│   ├── Bss.Core.UserValues        # Core handling of user-specific values
	│   ├── Bss.Dal                    # Data Access Layer, referencing core modules and implementing interfaces
	│       ├── CoreDbContext.cs       # Core database context
	│       ├── IdentityDbContext.cs   # Identity-specific database context
	│       ├── Module.cs              # DAL module registration and configuration
	│   ├── Bss.Infrastructure         # Infrastructure project for cross-module integration
	│   ├── Bss.UserValues             # User values specific functionality
	tools/
	│   ├── migration_creator/         # Tools for managing database migrations
	│       ├── add_migration.ps1      # Script to add a migration` 

## Key Components

### Bss.Core.Engine

The `Bss.Core.Engine` module is responsible for performing calculations that can be executed on any implemented language engine, making it flexible and adaptable for various processing needs.

### Cache-First Approach

The project follows a **cache-first** strategy, meaning that most of the logic and calculations are retrieved from cache to improve system speed. Since only the admin performs updates occasionally, changes in the database trigger an event, prompting our infrastructure to run a job that syncs the cache with the latest updates.

### Configuration Management

Configuration files in the repository are set up for **local development**. During the CI/CD deployment process, secrets and sensitive information are fetched from a vault, which overrides the local configurations. This ensures secure management of environment-specific settings and keeps secrets out of the source code.

### Modular Startup Configuration

Each module has its own startup file and configuration. This design ensures that only the necessary services and configurations are registered for each module. The **Bootstrap** project can reference as many modules as needed, starting them as part of the overall service.

### Commands and Events

The project follows best practices by using **commands** instead of controllers. Each module can define its own set of commands, events, and cron jobs. For example:

-   **Identity Module**: Manages sign-in, sign-up, and default user provisioning.
-   **Admin Module**: Contains administrative commands specific to user and system management.

### Infrastructure Project

The **Infrastructure** project enables modules to run independently yet work seamlessly with other modules when combined. It includes shared configurations and tools like:

-   **Hangfire**: Manages cron jobs and scheduled tasks.
-   **Swagger**: Provides a UI for API documentation and testing.

## Authentication

The application integrates with **Google** and **GitHub** for sign-in functionality, allowing users to authenticate securely using their existing accounts.

## Technologies Used

-   **.NET 8**: Core framework for building modular microservices.
-   **Hangfire**: Manages cron jobs within the service.
-   **Swagger**: Enhances API usability for development and testing.

## Contributing

Feel free to fork the repository and create a pull request if you'd like to contribute.