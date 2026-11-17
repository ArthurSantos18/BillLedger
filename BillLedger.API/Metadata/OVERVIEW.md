# 📡 BillLedger API

API REST para gestão de faturamento e cobrança.

**Base URL:** `https://localhost:7050` (ajuste para a sua porta local)
**Documentação interativa:** `/swagger` ou `/scalar`

---

## 🩺 Observabilidade e Status

A API expõe endpoints para monitoramento de saúde e resiliência.

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/health/live` | Liveness Check (Processo está vivo?) |
| `GET` | `/health/ready` | Readiness Check (Banco PostgreSQL acessível?) |

---

## 📋 Endpoints

### Customers (Clientes)
| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/api/customers` | Cria um novo cliente |
| `GET` | `/api/customers` | Lista clientes (Paginado e com filtros) |
| `GET` | `/api/customers/{id}` | Busca cliente por ID |
| `PATCH` | `/api/customers/{id}/rename` | Altera o nome do cliente |
| `DELETE` | `/api/customers/{id}` | Remove o cliente (Soft Delete) |

### Invoices (Faturas)
| Método | Rota | Descrição |
|--------|------|-----------|
| `POST` | `/api/invoices` | Cria uma nova fatura (Status: Draft) |
| `GET` | `/api/invoices` | Lista faturas (Paginado e com filtros) |
| `GET` | `/api/invoices/{id}` | Busca fatura por ID |
| `POST` | `/api/invoices/{id}/issue` | Emite a fatura (Draft -> Issued) |
| `POST` | `/api/invoices/{id}/pay` | Registra pagamento (Issued -> Paid) |
| `POST` | `/api/invoices/{id}/cancel` | Cancela a fatura (Draft/Issued -> Cancelled) |

---

## 🔍 Filtros de Busca (Listagens)

### Para `GET /api/customers`
- `name`: Busca parcial pelo nome (case-insensitive)
- `email`: Busca exata pelo e-mail
- `cpf`: Busca exata pelo CPF
- `isActive`: Filtra por status ativo/inativo (true/false)

### Para `GET /api/invoices`
- `customerId`: Filtra pelo ID do cliente
- `status`: Filtra pelo status (Draft, Issued, Paid, Cancelled)
- `minDueDate` / `maxDueDate`: Filtra por período de vencimento
- `minTotalAmount` / `maxTotalAmount`: Filtra por faixa de valor

---

## 📄 Paginação

Todas as listagens retornam o mesmo envelope de paginação:

```json
{
  "page": 1,
  "pageSize": 60,
  "totalResults": 150,
  "totalPages": 3,
  "hasPrevious": false,
  "hasNext": true,
  "results": []
}
```

**Parâmetros na URL:** `?page=1&pageSize=60` (Padrão: `page=1`, `pageSize=60`).

---

## ⚠️ Formato de Erro

A API segue estritamente o padrão **RFC 7807 (ProblemDetails)** para todas as respostas de erro.

### Erros de Negócio e Validação (400, 404, 409)

**Exemplo: Not Found (404)**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Não Encontrado",
  "status": 404,
  "detail": "Cliente não encontrado.",
  "instance": "/api/customers/3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "correlationId": "0HN7K9J2L3M4N"
}
```

**Exemplo: Erro de Validação do FluentValidation (400)**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Erro de Validação",
  "status": 400,
  "detail": "Validation failed: \n -- Request.Name: O nome é obrigatório.",
  "instance": "/api/customers",
  "correlationId": "0HN7K9J2L3M4N",
  "errors": {
    "Request.Name": [
      "O nome é obrigatório."
    ]
  }
}
```

**Exemplo: Conflito de Estado (409)**
*(Ex: Tentar pagar uma fatura já paga)*
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.8",
  "title": "Conflito de Estado",
  "status": 409,
  "detail": "Fatura já está paga.",
  "instance": "/api/invoices/3fa85f64.../pay",
  "correlationId": "0HN7K9J2L3M4N"
}
```

**Erros inesperados (500):** Retornam o mesmo formato, mas ocultam detalhes técnicos em produção e incluem `stackTrace` apenas em ambiente de desenvolvimento.

---

## 🚦 Rate Limiting

A API utiliza uma política Global de **Sliding Window** para proteção contra abusos.

| Policy | Endpoints | Limite | Janela | Particionamento |
| --- | --- | --- | --- | --- |
| `GlobalLimiter` | Todas as requisições | 100 req/min | 1 minuto | IP do cliente |

**Se exceder:** Retorna `429 Too Many Requests` com um JSON `ProblemDetails` e o header `Retry-After` indicando quantos segundos aguardar.

---