using Shared.Enums;

namespace Shared.Filters;

public record TicketFilteringParameters(string? Title, string? Description, TicketStatus? Status, Guid? BoardId);
