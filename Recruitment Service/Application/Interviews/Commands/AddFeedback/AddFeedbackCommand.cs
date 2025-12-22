using Application.Abstractions.Messaging;

namespace Application.Interviews.Commands.AddFeedback;

public sealed record AddFeedbackCommand(Guid Id, string Feedback, bool IsPassed) : ICommand;
