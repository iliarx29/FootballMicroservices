using MediatR;

namespace Matches.Application.Matches.Queries.AutocompleteSearch;
public record AutocompleteSearchQuery(string Query) : IRequest<List<string>>;
