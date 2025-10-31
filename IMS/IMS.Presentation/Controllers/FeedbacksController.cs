using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using IMS.Presentation.Routing;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Presentation.Controllers;

[ApiController]
[Route(ApiRoutes.Feedbacks.Base)]
public class FeedbacksController(IFeedbackService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<FeedbackDto>> GetAll(CancellationToken cancellationToken)
    {
        var feedbacks = await service.GetAllAsync(cancellationToken: cancellationToken); 

        var feedbackDTOs = mapper.Map<IEnumerable<FeedbackDto>>(feedbacks);
         
        return feedbackDTOs;
    }

    [HttpGet(ApiRoutes.Id)]
    public async Task<FeedbackDto> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var feedback = await service.GetByIdAsync(id, cancellationToken);

        var feedbackDTO = mapper.Map<FeedbackDto>(feedback);

        return feedbackDTO;
    }

    [HttpGet(ApiRoutes.Feedbacks.FeedbacksByTicketId)]
    public async Task<List<FeedbackDto>> GetFeedbacksByTicketId(Guid ticketId, CancellationToken cancellationToken)
    {
        var feedbacks = await service.GetFeedbacksByTicketIdAsync(ticketId, cancellationToken: cancellationToken);

        var feedbackDTOs = mapper.Map<List<FeedbackDto>>(feedbacks);

        return feedbackDTOs;
    }

    [HttpPost]
    public async Task<FeedbackDto> Create([FromBody] CreateFeedbackDto createFeedbackDTO, CancellationToken cancellationToken)
    {
        var feedbackModel = mapper.Map<FeedbackModel>(createFeedbackDTO);

        var createdFeedbackModel = await service.CreateAsync(feedbackModel, cancellationToken);

        var createdFeedbackDTO = mapper.Map<FeedbackDto>(createdFeedbackModel);

        return createdFeedbackDTO;
    }

    [HttpPut(ApiRoutes.Id)]
    public async Task<FeedbackDto> Update([FromRoute] Guid id, [FromBody] UpdateFeedbackDto updateFeedbackDTO, CancellationToken cancellationToken)
    {
        var feedbackModel = mapper.Map<FeedbackModel>(updateFeedbackDTO); 

        var updatedFeedbackModel = await service.UpdateAsync(id, feedbackModel, cancellationToken);

        var updatedFeedbackDTO = mapper.Map<FeedbackDto>(updatedFeedbackModel);

        return updatedFeedbackDTO;
    }
}
