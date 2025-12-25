using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Application.Candidates.Commands.UpdateCv;

public sealed record UpdateCvCommand(Guid CandidateId, IFormFile File) : ICommand;
