# Definitely Not Overengineered API

> **It's just an e-commerce API. How complicated could it be?**

An intentionally overengineered e-commerce API built as a learning project to explore **backend engineering, software architecture, distributed systems, DevOps, observability, security, and cloud infrastructure**.

The goal isn't just to build an API that works.

The goal is to understand **why production systems are designed the way they are**, what problems different technologies solve, their trade-offs, and what happens when things inevitably go wrong.

> **The e-commerce is just the excuse. The real project is learning backend engineering.**

---

## 🚧 Project Status

**Work in progress.**

The project is being developed incrementally, starting from the core e-commerce domain and evolving toward a production-oriented architecture.

Technologies and architectural patterns are intentionally introduced as the project grows.

---

## 🎯 What am I trying to learn?

This project is a practical laboratory for exploring:

* Backend architecture
* Vertical Slice Architecture
* Domain modeling
* REST API design
* Authentication & authorization
* Database modeling
* Transactions & concurrency
* Distributed caching
* Idempotency
* Asynchronous messaging
* Payment processing
* Webhooks
* Automated testing
* Containerization
* CI/CD
* Observability
* Application security
* Resilience & fault tolerance
* Performance optimization
* Cloud infrastructure

The project is intentionally overengineered **for learning purposes**.

In a real production system, every architectural decision should be justified by an actual requirement.

---

# 🏗️ Architecture

The project uses **Vertical Slice Architecture**, organizing code around business capabilities rather than traditional technical layers.

Instead of:

```text
Controllers/
Services/
Repositories/
```

the application is organized around features:

```text
Features/
├── Auth/
│   └── Signin/
├── Users/
│   ├── Create/
│   ├── FindOne/
│   ├── FindAll/
│   ├── Update/
│   └── Delete/
├── Products/
├── Categories/
└── Orders/
```

Shared concerns are separated into:

```text
Domain/
Data/
Infrastructure/
Features/
```

The goal is to keep use cases cohesive and minimize unnecessary coupling between unrelated parts of the application.

### High-level structure

```text
                    ┌──────────────┐
                    │    Client    │
                    └──────┬───────┘
                           │
                           ▼
                    ┌──────────────┐
                    │ ASP.NET Core │
                    │     API      │
                    └──────┬───────┘
                           │
              ┌────────────┼────────────┐
              │            │            │
              ▼            ▼            ▼
          PostgreSQL     Redis       RabbitMQ
              │                         │
              │                         ▼
              │                  Async Processing
              │
              ▼
          Domain/Data
```

---

# 🛠️ Tech Stack

### Backend

* C#
* .NET
* ASP.NET Core
* Entity Framework Core
* REST API

### Database

* PostgreSQL

### Infrastructure

* Docker
* Docker Compose
* Redis
* RabbitMQ
* Linux
* Reverse Proxy
* HTTPS

### Authentication & Security

* JWT
* Role-based authorization
* Password hashing
* Input validation
* Secure configuration
* Security hardening

### Payments

* Stripe
* Payment webhooks
* Idempotent payment processing

### Testing

* Unit Tests
* Integration Tests
* Testcontainers

### Observability

* Structured logging
* Metrics
* Distributed tracing
* OpenTelemetry
* Correlation IDs
* Health checks

### DevOps

* GitHub
* CI/CD
* Docker Images
* Linux deployment
* Cloud infrastructure

---

# 🛒 Domain

The project models a simplified e-commerce system.

```text
User
 │
 └── Order
      │
      └── OrderItem
            │
            └── Product
                   │
                   └── Category
```

## User

Represents a customer or administrator.

```text
User
├── Id
├── Name
├── Email
├── Password
└── Role
```

## Product

Represents an item available for purchase.

```text
Product
├── Id
├── Name
├── Description
├── Price
└── Stock
```

## Order

Represents a purchase.

```text
Order
├── Id
├── User
├── Address
├── Status
├── Total
└── Items
```

## OrderItem

Represents a product inside an order.

```text
OrderItem
├── Order
├── Product
├── Quantity
└── UnitPrice
```

`UnitPrice` is stored in the order item as a **price snapshot**.

This means that if a product costs `$50` today and `$70` tomorrow, an order placed today still knows that the customer paid `$50`.

This is a small example of the kind of domain decision this project is meant to explore.

---

# 🔐 Authentication

Authentication is based on JWT.

```text
Client
  │
  │ credentials
  ▼
Sign In
  │
  ▼
JWT
  │
  ▼
Authorization: Bearer <token>
  │
  ▼
ASP.NET Core
  │
  ├── Authentication
  │
  └── Authorization
          │
          ▼
       Endpoint
```

The API will support role-based authorization, such as:

```text
CUSTOMER
ADMIN
```

---

# ⚡ Redis & Caching

Redis will be introduced using the **cache-aside pattern**.

```text
GET /products/123
        │
        ▼
     Redis?
     /     \
   HIT     MISS
    │        │
    │        ▼
    │     PostgreSQL
    │        │
    │        ▼
    │      Redis
    │        │
    └────────┘
         │
         ▼
      Response
```

The first candidates for caching are read-heavy resources such as:

* Products
* Categories
* Users

Write operations will invalidate affected cache entries.

Redis will later also be explored for other use cases such as:

* Idempotency keys
* Rate limiting
* Session/refresh-token storage
* Shopping carts

---

# 📨 RabbitMQ & Asynchronous Processing

RabbitMQ will be used to explore event-driven and asynchronous processing.

For example:

```text
Order Created
     │
     ├──► Send Email
     │
     ├──► Update Inventory
     │
     ├──► Process Payment
     │
     └──► Publish Event
```

This opens the door to studying:

* Producers & consumers
* Message brokers
* Event-driven architecture
* Retry strategies
* Dead-letter queues
* At-least-once delivery
* Eventual consistency
* Failure handling

---

# 💳 Payments & Webhooks

Stripe will be used to explore payment processing and asynchronous payment confirmation.

```text
Client
  │
  ▼
Create Order
  │
  ▼
Stripe Checkout
  │
  ▼
Customer Payment
  │
  ▼
Stripe
  │
  │ Webhook
  ▼
API
  │
  ▼
Validate Event
  │
  ▼
Update Order
```

The API will not blindly trust the client to tell it whether a payment succeeded.

Webhooks will be validated and processed **idempotently**, because external systems may deliver the same event more than once.

---

# 🧪 Testing

Testing is treated as part of the engineering process rather than something added at the end.

## Unit Tests

Testing isolated business behavior:

```text
Handler
  │
  ├── Mock dependency
  ├── Execute use case
  └── Assert result
```

## Integration Tests

Testing real interactions between components:

```text
API
 │
 ▼
EF Core
 │
 ▼
PostgreSQL
```

## Testcontainers

External dependencies will eventually be tested using real containers.

The objective is to reduce the gap between:

> "The test passed."

and:

> "The system actually works with its dependencies."

---

# 🐳 Containerization

Docker Compose provides reproducible development infrastructure.

For example:

```bash
docker compose up -d
```

As the project evolves, production-oriented containerization will be introduced.

---

# 📊 Observability

The project will progressively introduce:

* Structured logs
* Metrics
* Distributed tracing
* OpenTelemetry
* Correlation IDs
* Health checks

The goal is to answer questions such as:

> What happened?

> Where did it happen?

> How long did it take?

> Which dependency caused the failure?

> Is the system actually healthy?

Because:

```text
"It doesn't work"
```

is not an observability strategy.

---

# 🚀 Deployment

The deployment roadmap looks roughly like this:

```text
GitHub
   │
   ▼
CI/CD
   │
   ▼
Docker Image
   │
   ▼
Linux Server
   │
   ├── Reverse Proxy
   ├── HTTPS
   ├── API
   ├── PostgreSQL
   ├── Redis
   └── RabbitMQ
```

Later stages will explore cloud infrastructure and production deployment strategies.

---

# 🗺️ Roadmap

### Foundation

* [x] Project foundation
* [x] User CRUD
* [x] Initial domain modeling
* [x] EF Core configuration
* [x] Vertical Slice Architecture

### Authentication

* [ ] JWT authentication
* [ ] Authorization
* [ ] Role-based access control
* [ ] Password security

### E-commerce

* [ ] Products
* [ ] Categories
* [ ] Orders
* [ ] OrderItems
* [ ] Inventory management

### Distributed Systems

* [ ] Transactions
* [ ] Concurrency control
* [ ] Redis
* [ ] Cache invalidation
* [ ] Idempotency
* [ ] RabbitMQ
* [ ] Retry strategies
* [ ] Dead-letter queues

### Payments

* [ ] Stripe integration
* [ ] Checkout
* [ ] Payment webhooks
* [ ] Idempotent webhook processing

### Testing

* [ ] Unit tests
* [ ] Integration tests
* [ ] Testcontainers
* [ ] Test infrastructure dependencies

### DevOps

* [ ] API documentation
* [ ] Production Docker setup
* [ ] Linux deployment
* [ ] Reverse proxy
* [ ] HTTPS
* [ ] CI/CD

### Observability & Reliability

* [ ] Structured logging
* [ ] Metrics
* [ ] Distributed tracing
* [ ] OpenTelemetry
* [ ] Health checks
* [ ] Resilience
* [ ] Failure testing
* [ ] Performance testing

### Cloud & Architecture

* [ ] Cloud deployment
* [ ] Advanced architecture experiments
* [ ] Architecture Decision Records
* [ ] Production-oriented documentation

---

# 🧠 Engineering Philosophy

The most important rule of this project:

> **Don't add technology because it looks good on a README.**

Every major architectural decision should answer:

```text
What problem are we solving?
        │
        ▼
What are the alternatives?
        │
        ▼
What are the trade-offs?
        │
        ▼
Why did we choose this solution?
        │
        ▼
How do we implement it?
        │
        ▼
How do we test it?
        │
        ▼
How do we measure it?
        │
        ▼
What did we learn?
```

Complexity is not a virtue.

If a simple solution is better, the simple solution wins.

The purpose of overengineering here is **to learn where engineering complexity actually comes from**.

---

# 🤔 What This Project Is Really About

This isn't an attempt to build the world's greatest e-commerce platform.

It's an attempt to answer questions like:

* What happens when two users buy the last item simultaneously?
* How do we prevent duplicate operations?
* When should work be synchronous or asynchronous?
* What happens when Redis goes down?
* What happens when RabbitMQ is unavailable?
* What happens when a message is delivered twice?
* How should payment webhooks be handled safely?
* How do we test infrastructure-dependent code?
* How do we observe a distributed system?
* How do we deploy and operate an API outside localhost?
* When does a design pattern actually solve a problem?
* When is a simpler architecture better?

The e-commerce domain is just the excuse.

**The real project is learning backend engineering.**

---

# 📚 Documentation

Architectural decisions, experiments, implementation notes, and lessons learned will be documented as the project evolves.

The repository is both:

* a software project;
* a learning laboratory;
* and a technical journal.

---

# 🇧🇷 Português

## Definitely Not Overengineered API

> **É só uma API de e-commerce. Quão complicado isso poderia ser?**

Uma API de e-commerce propositalmente superdimensionada, criada como projeto de estudo para explorar **engenharia de backend, arquitetura de software, sistemas distribuídos, DevOps, observabilidade, segurança e infraestrutura em cloud**.

O objetivo não é simplesmente construir uma API que funcione.

É entender **por que sistemas de produção são projetados da maneira que são**, quais problemas cada tecnologia resolve, quais são seus trade-offs e o que acontece quando as coisas inevitavelmente dão errado.

> **O e-commerce é apenas a desculpa. O verdadeiro projeto é aprender engenharia de backend.**

### Objetivos

Este projeto é um laboratório prático para estudar:

* Arquitetura de backend
* Vertical Slice Architecture
* Modelagem de domínio
* APIs REST
* Autenticação e autorização
* Banco de dados e transações
* Controle de concorrência
* Cache distribuído
* Idempotência
* Mensageria assíncrona
* Pagamentos e webhooks
* Testes automatizados
* Docker
* CI/CD
* Observabilidade
* Segurança
* Resiliência
* Performance
* Cloud

### Filosofia

A regra principal é:

> **Não adicionar tecnologia só porque ela fica bonita no README.**

Toda decisão arquitetural importante deve responder:

```text
Qual problema estamos resolvendo?
        ↓
Quais são as alternativas?
        ↓
Quais são os trade-offs?
        ↓
Por que escolhemos essa solução?
        ↓
Como implementamos?
        ↓
Como testamos?
        ↓
Como medimos?
        ↓
O que aprendemos?
```

Complexidade não é uma virtude.

Se uma solução simples for melhor, **a solução simples vence**.

### Estado atual

O projeto está em desenvolvimento e será evoluído gradualmente, começando pelo domínio principal e avançando para autenticação, estoque, concorrência, Redis, RabbitMQ, pagamentos, testes de integração, CI/CD, observabilidade e infraestrutura.

---

# 👨‍💻 Author

**Paulo Sérgio**

Full Stack Developer interested in backend engineering, distributed systems, software architecture, and building things that are probably more complicated than they need to be.

---

# ⭐ Why "Definitely Not Overengineered"?

Because every project needs a reasonable amount of architecture.

This one just happens to have:

```text
PostgreSQL
Redis
RabbitMQ
Stripe
Docker
JWT
OpenTelemetry
CI/CD
Integration Tests
Distributed Systems
Cloud Infrastructure
```

...for an e-commerce API.

But it's **definitely not overengineered**.

Probably.

---

## 📄 License

This project is primarily intended for educational and portfolio purposes.
