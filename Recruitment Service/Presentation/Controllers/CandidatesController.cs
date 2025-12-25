using Application.Candidates.Commands.AcceptCandidateToInternship;
using Application.Candidates.Commands.RegisterCandidate;
using Application.Candidates.Commands.UpdateCv;
using Application.Candidates.Queries.FindByEmail;
using Application.Candidates.Queries.FindById;
using Application.Candidates.Queries.GetAll;
using Application.Candidates.Queries.GetCandidateCvUrlById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using static Presentation.ApiRoutes.ApiRoutes;
using static Presentation.ApiConstants.Permissions;

namespace Presentation.Controllers;

[Authorize]
[Route(Candidates.Base)]
public sealed class CandidatesController(ISender sender) : ApiController(sender)
{
    [Authorize(CandidatesPermissions.Register)]
    [HttpPost(Candidates.Register)]
    public async Task<ActionResult<Guid>> Register([FromBody] RegisterCandidateCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }

    [Authorize(CandidatesPermissions.AcceptToInternship)]
    [HttpPut(Candidates.AcceptToInternship)]
    public async Task<ActionResult> AcceptToInternship([FromQuery] AcceptCandidateToInternshipCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.ToString());
    }

    [Authorize(CandidatesPermissions.ManageCandidates)]
    [HttpPut(Candidates.UpdateCv)]
    public async Task<ActionResult> UpdateCv([FromRoute] Guid id, IFormFile file, CancellationToken cancellationToken)
    {
        var command = new UpdateCvCommand(id, file);
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.ToString());
    }

    [Authorize(CandidatesPermissions.Read)]
    [HttpGet(Candidates.ByEmail)]
    public async Task<ActionResult<FindCandidateByEmailQueryResponse>> GetByEmail([FromRoute] string email, CancellationToken cancellationToken)
    {
        var query = new FindCandidateByEmailQuery(email);
        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }

    [Authorize(CandidatesPermissions.Read)]
    [HttpGet(Id)]
    public async Task<ActionResult<FindCandidateByIdQueryResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new FindCandidateByIdQuery(id);
        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }

    [Authorize(CandidatesPermissions.Read)]
    [HttpGet(Candidates.GetAll)]
    public async Task<ActionResult<GetAllCandidatesQueryResponce>> GetAll([FromQuery] GetAllCandidatesQuery query, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }

    [Authorize(CandidatesPermissions.Read)]
    [HttpGet(Candidates.GetCvUrl)]
    public async Task<ActionResult<string>> GetCvUrl([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCandidateCvUrlByIdQuery(id);
        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }
}
