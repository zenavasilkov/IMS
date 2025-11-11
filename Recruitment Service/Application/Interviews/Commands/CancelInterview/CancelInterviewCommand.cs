using Application.Abstractions.Messaging;

namespace Application.Interviews.Commands.CancelInterview;

public sealed record CancelInterviewCommand(Guid Id) : ICommand;
