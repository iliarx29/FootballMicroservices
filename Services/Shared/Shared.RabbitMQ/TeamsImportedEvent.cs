namespace Shared.RabbitMQ;
public record TeamsImportedEvent(List<TeamCreatedEvent> CreatedTeams);
