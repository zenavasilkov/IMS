using Shared.Enums;

namespace Shared.Filters;

public record InternshipFilteringParameters(
    Guid? InternId,
    Guid? MentorId,
    Guid? HrManagerId,
    DateTime? StartedAfter,
    DateTime? StartedBefore,
    InternshipStatus? Status);
