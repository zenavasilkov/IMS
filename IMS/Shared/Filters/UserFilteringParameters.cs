using Shared.Enums;

namespace Shared.Filters;

public record UserFilteringParameters(Role? Role, string? FirstName, string? LastName);
