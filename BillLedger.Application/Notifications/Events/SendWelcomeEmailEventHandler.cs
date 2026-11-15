namespace BillLedger.Application.Notifications.Events;

public sealed class SendWelcomeEmailEventHandler(
    IEmailService emailService,
    ILogger<SendWelcomeEmailEventHandler> logger)
    : IEventHandler<CustomerCreatedDomainEvent>
{
    public async Task HandleAsync(CustomerCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Sending welcome email to {Email}",
            domainEvent.Email);

        await emailService.SendEmailAsync(
            to: domainEvent.Email,
            subject: "Bem-vindo ao BillLedger!",
            body: $"Olá, seu cadastro com o CPF {domainEvent.Cpf} foi criado com sucesso. Aproveite nossos serviços!",
            cancellationToken: cancellationToken);
    }
}
