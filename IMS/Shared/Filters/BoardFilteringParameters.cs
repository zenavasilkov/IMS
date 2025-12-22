namespace Shared.Filters;

public record BoardFilteringParameters(string? Title, string? Description, Guid? CreatedById, Guid? CreatedToId);
