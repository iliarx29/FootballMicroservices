using MassTransit;
using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using Shared.RabbitMQ;

namespace Matches.Application.Consumers;

public class TeamCreatedEventConsumer : IConsumer<TeamCreatedEvent>
{
    private readonly IMatchesDbContext _dbContext;

    public TeamCreatedEventConsumer(IMatchesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<TeamCreatedEvent> context)
    {
        var data = context.Message;

        await _dbContext.Teams.AddAsync(new Team { Id = data.Id, Name = data.Name });
        await _dbContext.SaveChangesAsync();
    }
}
