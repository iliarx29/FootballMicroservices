using MassTransit;
using Matches.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shared.RabbitMQ;

namespace Matches.Application.Consumers;
public class TeamDeletedEventConsumer : IConsumer<TeamDeletedEvent>
{
    private readonly IMatchesDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    public TeamDeletedEventConsumer(IMatchesDbContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
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

        await _unitOfWork.SaveChangesAsync();
    }
}
