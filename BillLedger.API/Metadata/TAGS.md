## Customers

Endpoints para o gerenciamento de **clientes** do sistema. Responsável por manter o cadastro de quem recebe as faturas.

### Responsabilidades

- Cadastro de clientes com Nome, E-mail (Value Object) e CPF (Value Object)
- Consulta paginada com filtros (nome parcial, e-mail exato, CPF exato e status)
- Renomeação de clientes
- Remoção lógica (Soft Delete)
- Busca por ID para visualização de detalhes

### Regras de Negócio

- **Dados Únicos**: não é possível cadastrar dois clientes com o mesmo CPF ou o mesmo E-mail.
- **Validação de Value Objects**: o CPF (com dígitos verificadores) e o E-mail (formato) são validados no Domínio antes de serem persistidos.
- **Soft Delete**: os clientes removidos (`IsDeleted = true`) não aparecem nas listagens ou buscas, garantindo a integridade do histórico de faturas.

### Rate Limiting

Este grupo está sujeito à política global da API:

- **100 requisições por minuto** por IP (Sliding Window)

---

## Invoices

Endpoints para o **ciclo de vida das faturas**. O coração do sistema de faturamento, que une o cliente ao valor a ser pago.

### Responsabilidades

- Criação de faturas (sempre iniciam no estado `Draft`)
- Consulta paginada com filtros (cliente, status, faixas de valores e período)
- Emissão da fatura para o cliente
- Registro de pagamento
- Cancelamento de faturas
- Busca por ID

### Comportamento (Máquina de Estados)

O domínio aplica regras estritas de transição de estado. Uma fatura pode assumir os seguintes estados:

- **Draft**: Fatura criada, mas não emitida. Não pode ser paga.
- **Issued**: Fatura emitida. Pode ser paga ou cancelada.
- **Paid**: Fatura paga. Não pode ser paga novamente ou cancelada.
- **Cancelled**: Fatura cancelada. Não pode ser paga.

### Regras de Negócio

- **Validações na Criação**: o valor deve ser maior que zero e a data de vencimento não pode estar no passado.
- **Conflito de Estado (HTTP 409)**: tentar pagar uma fatura `Draft` ou cancelar uma fatura `Paid` lança uma `StateConflictException` capturada pelo Global Exception Handler.
- **Monolito Modular**: quando uma fatura é emitida, paga ou cancelada, o domínio dispara **Domain Events** (ex: `InvoicePaidDomainEvent`) que são processados de forma assíncrona pelo módulo de Notificações.

### Rate Limiting

Este grupo está sujeito à política global da API:

- **100 requisições por minuto** por IP (Sliding Window)

---

## Notifications

Este módulo lida com a **comunicação com o cliente**. 

*Nota: Este módulo não expõe endpoints HTTP (Rotas). Ele opera nos bastidores, consumindo os eventos do sistema.*

### Responsabilidades

- Escutar os Domain Events disparados pelo módulo de Customers e Invoices via barramento (LiteBus).
- Simular o envio de e-mails transacionais (através do `MockEmailService`).

### Comportamento

O módulo de Notificações é totalmente desacoplado do faturamento. Ele reage aos seguintes eventos:

- `CustomerCreatedDomainEvent`: Envia e-mail de boas-vindas ao cliente.
- `InvoiceIssuedDomainEvent`: Envia e-mail com instruções de pagamento.
- `InvoicePaidDomainEvent`: Envia e-mail com o recibo de pagamento.
- `InvoiceCancelledDomainEvent`: Envia e-mail avisando sobre o cancelamento.

### Regras de Negócio

- **Pós-Commit**: os e-mails só são disparados após a confirmação do `SaveChanges` no banco de dados. Se a transação falhar, nenhum e-mail é enviado.

---