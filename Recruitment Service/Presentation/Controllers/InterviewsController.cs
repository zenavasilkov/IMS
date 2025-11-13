using Application.Interviews.Commands.AddFeedback;
using Application.Interviews.Commands.CancelInterview;
using Application.Interviews.Commands.RescheduleInterview;
using Application.Interviews.Commands.ScheduleInterview;
using Application.Interviews.Queries.GetAll;
using Application.Interviews.Queries.GetInterviewById;
using Application.Interviews.Queries.GetInterviewsByCandidateId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pagination;
using Presentation.Abstractions;
using static Presentation.ApiRoutes.ApiRoutes;
using static Presentation.ApiRoutes.ApiRoutes.Interviews;

namespace Presentation.Controllers;

[Route(Base)]
public class InterviewsController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Schedule([FromBody] ScheduleInterviewCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }

    [HttpPut(Interviews.Reschedule)]
    public async Task<ActionResult> Reschedule([FromBody] RescheduleInterviewCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.ToString());
    }

    [HttpPut(Interviews.Cancel)]
    public async Task<ActionResult> Cancel([FromQuery] CancelInterviewCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.ToString());
    }

    [HttpPut(Interviews.AddFeedback)]
    public async Task<ActionResult> AddFeedback([FromBody] AddFeedbackCommand command, CancellationToken cancellationToken)
    {
        var result = await Sender.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error.ToString());
    }

    [HttpGet(Id)]
    public async Task<ActionResult<GetInterviewByIdQueryResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetInterviewByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }

    [HttpGet(ByCandidateId)]
    public async Task<ActionResult<GetInterviewsByCandidateIdQueryResponse>> GetByCandidateId(
        [FromQuery] Guid candidateId,
        [FromQuery] PaginationParameters paginationParameters,
        CancellationToken cancellationToken)
    {
        var query = new GetInterviewsByCandidateIdQuery(candidateId, paginationParameters);

        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.ToString());
    }

    [HttpGet(Interviews.GetAll)]
    public async Task<ActionResult<GetAllInterviewsQueryResponse>> GetAll([FromQuery] GetAllInterviewsQuery query,  CancellationToken cancellationToken)
    {
        var result = await Sender.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result?.Error.ToString());
    }
}
