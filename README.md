# StackOverflowLite

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-316192?style=for-the-badge&logo=postgresql)
![Redis](https://img.shields.io/badge/Redis-7-DC382D?style=for-the-badge&logo=redis)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

A lightweight clone of StackOverflow built as a RESTful API using **.NET 10** and **Clean Architecture**. This project serves as a robust backend providing essential Q&A platform functionalities such as user authentication, asking questions, posting answers, and managing tags. 

---

## 📋 Table of Contents
1. [Project Overview](#-project-overview)
2. [Architecture](#-architecture)
3. [Features](#-features)
4. [Folder Structure](#-folder-structure)
5. [Setup Instructions](#-setup-instructions)
6. [Docker Instructions](#-docker-instructions)
7. [Environment Variables](#-environment-variables)
8. [API Documentation](#-api-documentation)

---

## 🚀 Project Overview

**StackOverflowLite** is a backend API designed to demonstrate modern C# and .NET web development best practices. It implements a scalable and maintainable system utilizing CQRS, Domain-Driven Design (DDD) principles, and Docker for containerized deployment. PostgreSQL is used as the primary relational database, while Redis handles distributed caching.

---

## 🏗️ Architecture

The project strictly follows **Clean Architecture** principles to separate concerns and ensure maintainability:

- **Domain Layer:** Contains enterprise logic and entities (e.g., Question, Answer, User, Tag).
- **Application Layer:** Contains business logic, interfaces, CQRS commands/queries, and DTOs.
- **Infrastructure Layer:** Implements data access (Entity Framework Core), database migrations, and external services (Redis caching).
- **API Layer (Presentation):** ASP.NET Core Web API controllers routing HTTP requests to the Application layer.

---

## ✨ Features

- **Authentication & Authorization:** JWT-based user authentication (Register, Login).
- **Questions Management:** Users can post, read, update, and delete questions.
- **Answers Management:** Users can submit answers to specific questions.
- **Tags System:** Categorize questions with multiple tags.
- **Caching:** Redis integration for performant data retrieval.
- **Containerized:** Ready-to-use `docker-compose` environment for database and caching dependencies.
- **Health Checks:** Built-in Docker health checks for PostgreSQL and Redis.

---

## 📁 Folder Structure

```text
StackOverflowLite/
│
├── src/
│   ├── StackOverflowLite.Api/           # Controllers, Middleware, API setup
│   ├── StackOverflowLite.Application/   # CQRS Handlers, DTOs, Interfaces
│   ├── StackOverflowLite.Domain/        # Entities, Enums, Value Objects
│   └── StackOverflowLite.Infrastructure/# EF Core DbContext, Repositories, Redis
│
├── docs/                                # Project documentation (features, fixes, infra)
├── docker/                              # Docker-related configurations
├── docker-compose.yml                   # Multi-container Docker orchestration
├── Dockerfile                           # API containerization recipe
└── StackOverflowLite.slnx               # Solution file
```

---

## 💻 Setup Instructions

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker & Docker Desktop](https://www.docker.com/products/docker-desktop)
- IDE (Visual Studio 2022, JetBrains Rider, or VS Code)

### How to Clone and Run

1. **Clone the repository:**
   ```bash
   git clone https://github.com/MehedisCode/StackOverflowLite.git
   cd StackOverflowLite
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore StackOverflowLite.slnx
   ```

3. **Spin up the database and cache:**
   ```bash
   docker-compose up -d postgres redis
   ```
   *(This starts PostgreSQL on port 5433 and Redis on port 6379)*

4. **Apply Entity Framework Migrations (if applicable):**
   ```bash
   cd src/StackOverflowLite.Infrastructure
   dotnet ef database update --startup-project ../StackOverflowLite.Api
   ```

5. **Run the API:**
   ```bash
   cd src/StackOverflowLite.Api
   dotnet run
   ```

---

## 🐳 Docker Instructions

To run the entire stack (API, Database, and Cache) seamlessly in isolated containers:

1. Build and start all services in detached mode:
   ```bash
   docker-compose up --build -d
   ```

2. Check the logs to ensure the API is running:
   ```bash
   docker-compose logs -f api
   ```

3. To stop and remove the containers:
   ```bash
   docker-compose down
   ```

---

## ⚙️ Environment Variables

The application relies on the following environment variables (typically set in `appsettings.json`, `appsettings.Development.json`, or `docker-compose.yml`):

| Variable Name | Description | Default / Example |
|---------------|-------------|-------------------|
| `ASPNETCORE_ENVIRONMENT` | Application Environment | `Development` |
| `ConnectionStrings__DefaultConnection` | PostgreSQL connection string | `Host=postgres;Port=5432;Database=StackOverflowLiteDb;...` |
| `ConnectionStrings__redis` | Redis connection string | `redis:6379` |

---

## 📖 API Documentation

Once the API is running, you can interact with the endpoints. Swagger UI is typically available at `/swagger` in the development environment.

### Core Endpoints Overview

- **Auth:**
  - `POST /api/auth/register` - Register a new user
  - `POST /api/auth/login` - Authenticate and receive JWT
- **Questions:**
  - `GET /api/questions` - Retrieve a paginated list of questions
  - `GET /api/questions/{id}` - Retrieve a specific question with its answers
  - `POST /api/questions` - Ask a new question
- **Answers:**
  - `POST /api/questions/{questionId}/answers` - Submit an answer to a question
  - `PUT /api/answers/{id}` - Update an existing answer
- **Tags:**
  - `GET /api/tags` - Retrieve popular tags
  - `GET /api/tags/{name}/questions` - Get questions associated with a tag
- **Users:**
  - `GET /api/users/{id}` - Get a user's profile and reputation

*(Detailed request/response schemas can be explored directly in the local Swagger UI).*

---

Made with ❤️ by [MehedisCode](https://github.com/MehedisCode).
