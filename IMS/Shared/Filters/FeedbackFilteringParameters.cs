namespace Shared.Filters;

public record FeedbackFilteringParameters(Guid? TicketId, Guid? SentById, Guid? SentToId, string? Comment);
