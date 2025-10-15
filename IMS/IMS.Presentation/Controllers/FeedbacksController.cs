using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using IMS.Presentation.Routing;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Presentation.Controllers;

[ApiController]
[Route(ApiRoutes.Feedbacks.Base)]
public class FeedbacksController(IService<FeedbackModel, Feedback> service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<FeedbackDTO>> GetAll(CancellationToken cancellationToken)
    {
        var feedbacks = await service.GetAllAsync(cancellationToken: cancellationToken); 

        var feedbackDTOs = mapper.Map<IEnumerable<FeedbackDTO>>(feedbacks);
         
        return feedbackDTOs;
    }

    [HttpGet(ApiRoutes.Id)]
    public async Task<FeedbackDTO> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var feedback = await service.GetByIdAsync(id, cancellationToken);

        var feedbackDTO = mapper.Map<FeedbackDTO>(feedback);

        return feedbackDTO;
    }

    [HttpPost]
    public async Task<FeedbackDTO> Create([FromBody] CreateFeedbackDTO createFeedbackDTO, CancellationToken cancellationToken)
    {
        var feedbackModel = mapper.Map<FeedbackModel>(createFeedbackDTO);

        var createdFeedbackModel = await service.CreateAsync(feedbackModel, cancellationToken);

        var createdFeedbackDTO = mapper.Map<FeedbackDTO>(createdFeedbackModel);

        return createdFeedbackDTO;
    }

    [HttpPut(ApiRoutes.Id)]
    public async Task<FeedbackDTO> Update([FromRoute] Guid id, [FromBody] UpdateFeedbackDTO updateFeedbackDTO, CancellationToken cancellationToken)
    {
        var feedbackModel = mapper.Map<FeedbackModel>(updateFeedbackDTO); 

        var updatedFeedbackModel = await service.UpdateAsync(id, feedbackModel, cancellationToken);

        var updatedFeedbackDTO = mapper.Map<FeedbackDTO>(updatedFeedbackModel);

        return updatedFeedbackDTO;
    }
}
