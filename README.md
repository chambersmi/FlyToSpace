
# 🚀 FlyToSpace

FlyToSpace is a full-stack tour booking web application built with Angular, ASP.NET Core, Redis, and Docker. Users can explore space-themed tours, add them to a cart, and finalize bookings via a checkout process.

## 🧰 Prerequisites

- Docker
- Node.js (only if running Angular outside of Docker)
- .NET 8 SDK (only if running API outside of Docker)

## 🐳 Running the App with Docker Compose

### 📁 Project Structure

```
FlyToSpace/
│
├── client/                 # Angular frontend
│   ├── Dockerfile
│   └── ...
│
├── API/                    # ASP.NET Core backend
│   ├── Dockerfile
│   └── ...
│
├── Data/                   # Shared data volume for persistence
│
├── docker-compose.yml
└── README.md
```

### ▶️ To Start the App

In the root of the project directory (where `docker-compose.yml` is located), run:

```bash
docker-compose up --build
```

This will:

- Build and start the Angular frontend, exposed at `http://localhost:4200`
- Build and start the ASP.NET Core API, available at `http://localhost:5050`
- Start a Redis instance on port `6379`

### 🔄 Rebuilding After Changes

If you make changes to the code and want to rebuild the containers:

```bash
docker-compose up --build
```

To stop the services:

```bash
docker-compose down
```

## 🛠️ Notes

- If you encounter HTTPS issues with the backend, you can disable HTTPS in the ASP.NET project or properly generate and trust a developer certificate using `dotnet dev-certs https --trust`.
- The frontend uses `/api` as the base for API calls. Ensure it maps correctly to the backend container.

