# Service Marketplace Platform (MVP)

## 📌 Overview

This project is a fullstack Service Marketplace Platform where:

* Customers can create service requests
* Providers can discover and accept nearby requests
* The system enforces role-based access control (RBAC)
* Subscription limits are applied
* AI is used to enhance user input

The goal of this project is to demonstrate practical backend design, clean API structure, and real-world decision making.

---

## 🏗️ Architecture

### Backend

* ASP.NET Core Web API
* Clean Architecture principles
* CQRS Pattern (Commands / Queries separation)
* Result Pattern for consistent error handling

### Frontend

* Angular (Standalone Components + Signals)
* Role-based UI separation
* Guards for route protection

### Database

* SQL Server
* Structured schema with relations between:

  * Users
  * Roles
  * Requests

### Deployment

* Dockerized frontend (Nginx)
* Backend runs via .NET container

---

## 🔐 Authentication & Authorization

### Authentication

* JWT-based authentication

### RBAC (Role-Based Access Control)

System supports three roles:

* Admin
* Provider
* Customer

Access is enforced at the API level using:

* Role-based authorization attributes

### Example

* Only Providers can accept requests
* Only Customers can create requests
* Admin can manage users and roles

---

## ⚙️ Core Features

### Customer

* Create service request (title, description, location)
* View personal requests
* Request lifecycle tracking

### Provider

* View available requests
* Filter nearby requests (based on geolocation)
* Accept requests
* Update request status (accepted → completed)

### Request Lifecycle

```
pending → accepted → completed
```

State transitions are validated on the backend.

---

## 📍 Geolocation

* Each request stores latitude & longitude
* Nearby requests are retrieved using distance-based filtering

---

## 💳 Subscription System

* Free users: up to 3 requests
* Paid users: unlimited requests

Enforced on backend to prevent bypassing from frontend.

---

## 🤖 AI Integration

AI endpoint is used to:

* Enhance request descriptions
* Suggest category
* Suggest pricing

This improves request quality and user experience.

---

## 🌐 API Design

The API follows REST principles.

### Example Endpoints

* `POST /api/requests`
* `GET /api/requests/my`
* `GET /api/requests/nearby`
* `POST /api/requests/{id}/accept`
* `PATCH /api/requests/{id}/status`

---

## 🧠 Key Design Decisions

### 1. CQRS Pattern

Separates read and write operations for better scalability and maintainability.

### 2. Result Pattern

Avoids throwing exceptions for business logic, returning structured success/failure responses.

### 3. Backend Enforcement

All critical rules are enforced server-side:

* Subscription limits
* Role permissions
* Request state transitions

### 4. Simple RBAC

Used role-based authorization for simplicity and clarity in MVP stage.

---

## ⚖️ Trade-offs

* Chose role-based RBAC instead of dynamic permissions for simplicity
* Did not implement full SSR (used SPA for frontend stability)
* Used basic geolocation instead of advanced spatial indexing

---

## 🚀 How to Run

### Prerequisites

* Docker
* Node.js
* .NET SDK

---

### Backend

```
cd backend
dotnet run
```

---

### Frontend (Docker)

```
docker build -t angular-app .
docker run -p 8080:80 angular-app
```

Access:

```
http://localhost:8080
```

---

## 📄 API Documentation

* apidog available at:

```
https://edep6z06vy.apidog.io
```

---

## 🔮 Future Improvements

If given more time:

* Implement dynamic permission-based RBAC
* Add real-time updates (WebSockets)
* Improve geolocation using spatial indexing
* Add caching layer
* Add background jobs for AI processing
* Improve logging & monitoring

---

## ✅ Conclusion

This project focuses on delivering a clean, functional MVP while making practical engineering trade-offs.

It demonstrates:

* Strong backend fundamentals
* Clear API design
* Real-world constraints handling
* End-to-end system thinking
