using Application.Abstractions.Messaging;

namespace Application.Interviews.Commands.RescheduleInterview;

public sealed record RescheduleInterviewCommand(Guid Id, DateTime NewDate) : ICommand; 
