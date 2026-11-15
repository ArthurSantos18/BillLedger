# 🧾 BillLedger - Sistema de Faturamento e Cobrança

[![CI](https://github.com/ArthurSantos18/BillLedger/actions/workflows/ci.yml/badge.svg)](https://github.com/ArthurSantos18/BillLedger/actions/workflows/ci.yml)
[![.NET](https://img.shields.io/badge/.NET-10-purple)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue)](LICENSE.txt)

## 📋 Sobre o Projeto

O **BillLedger** é um sistema backend de faturamento e cobrança (Billing/Invoicing) desenvolvido para gerenciar o ciclo de vida completo de clientes e faturas. O sistema permite o cadastro de clientes, emissão de faturas, registro de pagamentos e cancelamentos, com regras de negócio rigorosas.

Este projeto foi construído como portfólio focado em **Domain-Driven Design (DDD híbrido)**, **CQRS**, **Clean Architecture** e padrões consolidados de mercado.

---

## 🚀 Tecnologias Utilizadas

| Tecnologia | Finalidade |
|------------|------------|
| .NET 10 | Plataforma de desenvolvimento |
| C# | Linguagem principal |
| Entity Framework Core | ORM para acesso a dados |
| PostgreSQL | Banco de dados relacional (Npgsql) |
| LiteBus | Barramento de mensagens para CQRS e Domain Events |
| Mapster | Mapeamento rápido de DTOs (sem Reflection) |
| FluentValidation | Validação de requests na borda da API |
| Serilog | Logging estruturado (console) |
| Scalar / Swagger | Documentação interativa da API |
| Testcontainers | Containers PostgreSQL nos testes de integração |
| xUnit v3 | Framework de testes (unitários e integração) |
| AutoFixture | Geração de dados aleatórios para testes |
| Moq | Mocking de dependências nos testes |
| FluentAssertions | Asserções legíveis nos testes |
| Health Checks | Monitoramento de saúde da API (Liveness/Readiness) |
| Docker | Containerização do banco de dados local |

---

## 📁 Estrutura do Projeto

O projeto segue uma estrutura **flat** (sem pastas `src/` ou `tests/`), com separação clara por camadas e **Monolito Modular** na camada de Application.

```text
BillLedger/
├── BillLedger.API/                 # Camada de Apresentação (Controllers, Middleware)
│   ├── Controllers/
│   ├── Extensions/
│   ├── Filters/
│   ├── Metadata/
│   ├── Middleware/
│   └── Transformers/
├── BillLedger.Application/         # Camada de Aplicação (Handlers, Commands, Queries)
│   ├── Billing/
│   ├── Common/                
│   ├── Customers/
│   ├── Extensions/                                 
│   └── Notifications/              
├── BillLedger.Domain/              # Camada de Domínio (Entidades, Enums, Value Objects)
│   ├── Entities/                   
│   ├── Enums/                      
│   ├── Events/                     
│   ├── Exceptions/                 
│   ├── Patterns/                   
│   └── ValueObject/                
├── BillLedger.Infrastructure/      # Camada de Infraestrutura (DbContext, Repositories)
│   ├── Data/
│   ├── Extensions/
│   ├── Repositories/              
│   └── Services/                  
├── BillLedger.UnitTests/           # Testes unitários (Handlers, Validators, Customizations)
├── BillLedger.IntegrationTests/    # Testes de integração HTTP com banco real (Testcontainers)
```
---

## 🧩 Entidades Principais

| Entidade | Descrição |
|----------|-----------|
| **Customer** | Cliente do sistema. Possui Nome, E-mail (VO) e CPF (VO). |
| **Invoice** | Fatura. Possui valor monetário (VO), data de vencimento e um status controlado por máquina de estados. |
| **Domain Events** | Eventos que notificam o sistema sobre fatos passados (ex: `InvoicePaidDomainEvent`). |

---

## 🎯 Funcionalidades

### ✅ CRUDs e Ações de Negócio

- [x] **Customers** – Cadastro (Create), Renomeação (Rename), Exclusão Lógica (Delete), Busca por ID e Listagem Paginada com filtros.
- [x] **Billing (Invoices)** – Criação de fatura no estado `Draft`.
- [x] **Ciclo de Vida da Fatura**:
  - `Issue`: Emite a fatura (Draft -> Issued).
  - `Pay`: Registra o pagamento (Issued -> Paid).
  - `Cancel`: Cancela a fatura (Draft/Issued -> Cancelled).
- [x] **Listagem e Filtros** – Paginação e filtros dinâmicos (status, cliente, datas, valores).

### 📨 Domain Events e Notificações (Monolito Modular)

- [x] **CustomerCreated** – Dispara e-mail de boas-vindas.
- [x] **InvoiceIssued** – Dispara e-mail com boleto/instruções.
- [x] **InvoicePaid** – Dispara e-mail de recibo de pagamento.
- [x] **InvoiceCancelled** – Dispara e-mail avisando sobre o cancelamento.
- [x] **Desacoplamento** – O módulo de Billing não conhece o módulo de Notifications. Eles se comunicam exclusivamente via LiteBus.

### 📊 Observabilidade e Resiliência

- [x] **Global Exception Handler** – Middleware centralizado que traduz `StateConflictException` (HTTP 409), `DomainException`/`ValidationException` (HTTP 400) em `ProblemDetails` (RFC 7807).
- [x] **Correlation ID** – Header `X-Correlation-Id` injetado em todas as requisições e propagado nos logs e erros.
- [x] **Result Pattern** – Erros de negócio esperados (ex: NotFound) são retornados via `Result.Failure`, sem uso de exceptions.
- [x] **Rate Limiting** – Sliding Window de 100 requisições por minuto por IP (Nativo do .NET 10).
- [x] **Health Checks** – Endpoints `/health/live` (self) e `/health/ready` (PostgreSQL).

### 📦 Padrões e Boas Práticas

**Arquitetura e Domínio**
- **DDD Híbrido** – Entidades ricas com construtores privados, setters privados e métodos de fábrica. Value Objects para tipos complexos (`Money`, `Cpf`, `Email`).
- **Soft Delete** – Filtro global no EF Core (`HasQueryFilter`) para registros com `IsDeleted = true`.
- **Domain Events Interceptor** – O EF Core intercepta a lista de eventos da entidade e publica no LiteBus automaticamente após o `SaveChanges`.
- **Complex Properties** – Mapeamento nativo do EF Core 8+ para o Value Object `Money`.

**Validação e Configuração**
- **FluentValidation** – Validação de inputs na borda da API via `IAsyncActionFilter`.
- **Defesa em Profundidade** – Validação na API (FluentValidation) + Invariantes no Value Object (`DomainException`).
- **Migrations** – Versionamento do schema via EF Core Migrations.

---

## 🩺 Health Checks

O projeto expõe endpoints de health check no padrão:

| Endpoint | Tipo | Verifica | Ação em caso de falha |
|----------|------|----------|----------------------|
| `/health/live` | Liveness | Processo está vivo | Orquestrador **reinicia** o container |
| `/health/ready` | Readiness | PostgreSQL acessível | Orquestrador **remove do load balancer** |

---

## 🚦 Rate Limiting

A API utiliza **Rate Limiting** para limitar a quantidade de requisições recebidas e proteger os recursos da aplicação contra excesso de tráfego (DDoS, Scrapers).

| Policy | Endpoints | Limite | Janela | Particionamento |
| --- | --- | --- | --- | --- |
| `GlobalLimiter` | Todas as requisições | 100 req/min | 1 minuto (Sliding Window) | IP |

---

## 🐳 Executando com Docker (Banco de Dados)

A forma mais simples de rodar o projeto localmente é via **Docker Compose**, utilizado para subir o banco de dados PostgreSQL.

### Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e em execução.

### Passo a passo

**1. Clone o repositório**

```bash
git clone https://github.com/ArthurSantos18/BillLedger.git
cd BillLedger
```

**2. Configure as variáveis de ambiente**

```bash
cp .env.example .env
```

Edite o arquivo `.env` e preencha a senha do banco de dados:

```env
POSTGRES_PASSWORD=SuaSenhaForte@123
```

**3. Suba o container do banco de dados**

```bash
docker-compose up -d
```

O PostgreSQL estará disponível em `localhost:5432`.

---

## 🚀 Executando Localmente

### Pré-requisitos

- .NET 10.0 SDK
- PostgreSQL (ou via Docker)
- Git

### Passo a passo

**1. Clone o repositório**

```bash
git clone https://github.com/ArthurSantos18/BillLedger.git
cd BillLedger
```

**2. Configure a connection string**

Configure o arquivo `appsettings.Development.json` com sua connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=BillLedgerDb;Username=postgres;Password=SuaSenhaForte@123;"
  }
}
```

**3. Restaure os pacotes e compile o projeto**

```bash
dotnet restore
dotnet build
```

**4. Execute as migrações**

Crie o banco de dados e aplique as migrações existentes:

```bash
dotnet ef database update --project BillLedger.Infrastructure --startup-project BillLedger.API
```

**5. Execute a aplicação**

```bash
dotnet run --project BillLedger.API
```

**6. Acesse a documentação da API**

Abra o navegador e acesse:

```text
https://localhost:<porta>/scalar
```

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## ✒️ Autor

**Arthur Santos Azevedo**

- [![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?logo=linkedin&logoColor=white)](https://www.linkedin.com/)
- [![GitHub](https://img.shields.io/badge/GitHub-181717?logo=github&logoColor=white)](https://github.com/ArthurSantos18)

---

**BillLedger** 🚀
