using MassTransit;
using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using Shared.RabbitMQ;

namespace Matches.Application.Consumers;

public class TeamCreatedEventConsumer : IConsumer<TeamCreatedEvent>
{
    private readonly IMatchesDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public TeamCreatedEventConsumer(IMatchesDbContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task Consume(ConsumeContext<TeamCreatedEvent> context)
    {
        var data = context.Message;

        await _dbContext.Teams.AddAsync(new Team { Id = data.Id, Name = data.Name });
        await _unitOfWork.SaveChangesAsync();
    }
}
