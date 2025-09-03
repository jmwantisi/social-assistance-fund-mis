# Social Assistance Fund Management Information System (MIS)

A comprehensive, modern web application built with **Blazor Server** and **.NET 9** for managing social assistance fund applications, applicants, and related administrative tasks.

## 🚀 Features

- **Modern Architecture**: Clean architecture with separation of concerns
- **Blazor Server**: Interactive web UI with real-time updates
- **Entity Framework Core**: Modern ORM with code-first approach
- **REST API**: Full-featured API with Swagger documentation
- **Authentication & Authorization**: JWT-based security with role-based access
- **Validation**: Comprehensive input validation using FluentValidation
- **Logging**: Structured logging with Serilog
- **Caching**: In-memory caching for performance
- **Error Handling**: Global exception handling with custom middleware
- **Repository Pattern**: Clean data access layer
- **Unit of Work**: Transaction management
- **AutoMapper**: Object-to-object mapping
- **Health Checks**: Application health monitoring
- **CORS Support**: Cross-origin resource sharing
- **Response Compression**: Optimized response sizes

## 🏗️ Architecture

```
socialAssistanceFundMIS/
├── Components/           # Blazor components and pages
├── Controllers/          # REST API controllers
├── Data/                # Entity Framework context and models
├── DTOs/                # Data Transfer Objects
├── Infrastructure/      # Cross-cutting concerns
│   ├── Mapping/         # AutoMapper profiles
│   ├── Middleware/      # Custom middleware
│   ├── Repositories/    # Repository pattern implementation
│   └── Validation/      # FluentValidation validators
├── Models/              # Domain entities
├── Services/            # Business logic services
├── ViewModels/          # View-specific models
└── Common/              # Shared utilities and constants
```

## 🛠️ Technology Stack

- **.NET 9**: Latest .NET framework
- **Blazor Server**: Interactive web UI framework
- **Entity Framework Core 9**: Modern ORM
- **SQL Server**: Primary database
- **AutoMapper**: Object mapping
- **FluentValidation**: Input validation
- **Serilog**: Structured logging
- **Swagger/OpenAPI**: API documentation
- **Bootstrap**: UI framework
- **JWT**: Authentication tokens

## 📋 Prerequisites

- **.NET 9 SDK** or later
- **SQL Server** (Express, Developer, or Enterprise)
- **Visual Studio 2022** or **VS Code**
- **Git** for version control

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/social-assistance-fund-mis.git
cd social-assistance-fund-mis
```

### 2. Configure Database

1. Update the connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "SQLServerExpressEdition": "Server=localhost\\SQLEXPRESS;Database=SocialAssistanceDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

2. Run Entity Framework migrations:
```bash
dotnet ef database update
```

### 3. Install Dependencies

```bash
dotnet restore
```

### 4. Run the Application

```bash
dotnet run
```

The application will be available at:
- **Blazor UI**: https://localhost:5001
- **API Documentation**: https://localhost:5001/api
- **Health Check**: https://localhost:5001/health

## 🔧 Configuration

### Environment Variables

Create `appsettings.Development.json` for development-specific settings:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  },
  "ConnectionStrings": {
    "SQLServerExpressEdition": "your-dev-connection-string"
  }
}
```

### Authentication

Configure JWT authentication in `appsettings.json`:

```json
"Authentication": {
  "Authority": "https://your-auth-server.com",
  "Audience": "social-assistance-fund-mis"
}
```

## 📚 API Documentation

The application includes comprehensive API documentation via Swagger. Access it at `/api` when running in development mode.

### Key Endpoints

- **GET** `/api/applicants` - List all applicants (paginated)
- **GET** `/api/applicants/{id}` - Get applicant by ID
- **POST** `/api/applicants` - Create new applicant
- **PUT** `/api/applicants/{id}` - Update applicant
- **DELETE** `/api/applicants/{id}` - Delete applicant

## 🗄️ Database Schema

The system includes the following main entities:

- **Applicants**: Personal information and contact details
- **Applications**: Assistance fund applications
- **AssistancePrograms**: Available assistance programs
- **GeographicLocations**: Geographic hierarchy (regions, districts, villages)
- **Officers**: System users and administrators
- **Statuses**: Application and entity status tracking

## 🔒 Security Features

- **JWT Authentication**: Secure token-based authentication
- **Role-Based Authorization**: Admin, Officer, and Viewer roles
- **Input Validation**: Comprehensive validation using FluentValidation
- **HTTPS Enforcement**: Secure communication
- **CORS Configuration**: Controlled cross-origin access
- **Audit Trail**: Track all changes with timestamps and user information

## 📊 Logging

The application uses Serilog for structured logging:

- **Console Logging**: Development and debugging
- **File Logging**: Persistent log storage with rotation
- **Structured Data**: Rich logging context
- **Performance Monitoring**: Track application performance

## 🧪 Testing

### Unit Tests

```bash
dotnet test
```

### Integration Tests

```bash
dotnet test --filter Category=Integration
```

## 🚀 Deployment

### Production Deployment

1. Update `appsettings.Production.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "ConnectionStrings": {
    "SQLServerExpressEdition": "your-production-connection-string"
  }
}
```

2. Build and publish:
```bash
dotnet publish -c Release -o ./publish
```

3. Deploy to your hosting environment (IIS, Azure, etc.)

### Docker Deployment

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["socialAssistanceFundMIS.csproj", "./"]
RUN dotnet restore "socialAssistanceFundMIS.csproj"
COPY . .
RUN dotnet build "socialAssistanceFundMIS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "socialAssistanceFundMIS.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "socialAssistanceFundMIS.dll"]
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

- **Documentation**: [Wiki](https://github.com/your-username/social-assistance-fund-mis/wiki)
- **Issues**: [GitHub Issues](https://github.com/your-username/social-assistance-fund-mis/issues)
- **Email**: support@socialassistancefund.com

## 🔄 Changelog

### Version 1.0.0
- Initial release with core functionality
- Modern architecture implementation
- Comprehensive API endpoints
- Advanced validation and error handling
- Professional logging and monitoring

## 🙏 Acknowledgments

- **Blazor Team** for the amazing web framework
- **Entity Framework Team** for the robust ORM
- **Open Source Community** for the excellent libraries and tools

---

**Built with ❤️ for better social assistance management**


