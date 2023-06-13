using AutoMapper;
using MassTransit;
using Matches.Application.Abstractions;
using Matches.Domain.Entities;
using Shared.RabbitMQ;

namespace Matches.Application.Consumers;
public class TeamsImportedEventConsumer : IConsumer<TeamsImportedEvent>
{
    private readonly IMatchesDbContext _dbContext;
    private readonly IMapper _mapper;

    public TeamsImportedEventConsumer(IMatchesDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<TeamsImportedEvent> context)
    {
        var data = context.Message;

        var teams = _mapper.Map<List<Team>>(data.CreatedTeams);

        _dbContext.Teams.AddRange(teams);
        await _dbContext.SaveChangesAsync();
    }
}
