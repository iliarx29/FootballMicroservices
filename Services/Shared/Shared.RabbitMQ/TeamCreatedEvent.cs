namespace Shared.RabbitMQ;

public record TeamCreatedEvent
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
