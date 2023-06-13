using MassTransit;
using Matches.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shared.RabbitMQ;

namespace Matches.Application.Consumers;
public class TeamDeletedEventConsumer : IConsumer<TeamDeletedEvent>
{
    private readonly IMatchesDbContext _dbContext;
    public TeamDeletedEventConsumer(IMatchesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Consume(ConsumeContext<TeamDeletedEvent> context)
    {
        var data = context.Message;

        var team = await _dbContext.Teams.FirstOrDefaultAsync(x => x.Id == data.Id);

        if (team is null)
        {
            Console.WriteLine($"Team with id: '{data.Id}' doesn't exist.");
            return;
        }

        _dbContext.Teams.Remove(team);

        await _dbContext.SaveChangesAsync();
    }
}
