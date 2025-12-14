using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using IMS.Presentation.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Filters;
using Shared.Pagination;

namespace IMS.Presentation.Controllers;

[Authorize]
[ApiController]
[Route(ApiRoutes.Feedbacks.Base)]
public class FeedbacksController(IFeedbackService service, IMapper mapper) : ControllerBase
{
    [Authorize("read:feedbacks")]
    [HttpGet]
    public async Task<IEnumerable<FeedbackDto>> GetAll(
        [FromQuery] PaginationParameters paginationParameters,
        [FromQuery] FeedbackFilteringParameters filter,
        CancellationToken cancellationToken)
    {
        var feedbacks = await service.GetAllAsync(cancellationToken: cancellationToken); 

        var feedbackDtos = mapper.Map<IEnumerable<FeedbackDto>>(feedbacks);
         
        return feedbackDtos;
    }

    [Authorize("read:feedbacks")]
    [HttpGet(ApiRoutes.Id)]
    public async Task<FeedbackDto> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var feedback = await service.GetByIdAsync(id, cancellationToken);

        var feedbackDto = mapper.Map<FeedbackDto>(feedback);

        return feedbackDto;
    }

    [Authorize("create:feedbacks")]
    [HttpPost]
    public async Task<FeedbackDto> Create([FromBody] CreateFeedbackDto createFeedbackDto, CancellationToken cancellationToken)
    {
        var feedbackModel = mapper.Map<FeedbackModel>(createFeedbackDto);

        var createdFeedbackModel = await service.CreateAsync(feedbackModel, cancellationToken);

        var createdFeedbackDto = mapper.Map<FeedbackDto>(createdFeedbackModel);

        return createdFeedbackDto;
    }

    [Authorize("update:feedbacks")]
    [HttpPut(ApiRoutes.Id)]
    public async Task<FeedbackDto> Update([FromRoute] Guid id, [FromBody] UpdateFeedbackDto updateFeedbackDto, CancellationToken cancellationToken)
    {
        var feedbackModel = mapper.Map<FeedbackModel>(updateFeedbackDto); 

        var updatedFeedbackModel = await service.UpdateAsync(id, feedbackModel, cancellationToken);

        var updatedFeedbackDto = mapper.Map<FeedbackDto>(updatedFeedbackModel);

        return updatedFeedbackDto;
    }
}
