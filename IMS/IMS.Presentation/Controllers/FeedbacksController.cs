using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedbacksController(IService<FeedbackModel, Feedback> service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<FeedbackDTO>> GetAll(CancellationToken cancellationToken)
    {
        var feedbacks = await service.GetAllAsync(null, false, cancellationToken);

        if (feedbacks.Count == 0) throw new Exception($"No feedbacks have been found");

        var feedbackDTOs = mapper.Map<IEnumerable<FeedbackDTO>>(feedbacks);
         
        return feedbackDTOs;
    }

    [HttpGet("{id:guid}")]
    public async Task<FeedbackDTO> GetById(Guid id, CancellationToken cancellationToken)
    {
        var feedback = await service.GetByIdAsync(id, cancellationToken) ?? throw new Exception($"Feedback with ID {id} was not found.");

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

    [HttpPut("{id:guid}")]
    public async Task<FeedbackDTO> Update(Guid id, [FromBody] UpdateFeedbackDTO updateFeedbackDTO, CancellationToken cancellationToken)
    {
        var feedbackModel = mapper.Map<FeedbackModel>(updateFeedbackDTO);

        feedbackModel.Id = id;

        var updatedFeedbackModel = await service.UpdateAsync(feedbackModel, cancellationToken) ?? throw new Exception($"Feedback with ID {id} was not found.");

        var updatedFeedbackDTO = mapper.Map<FeedbackDTO>(updatedFeedbackModel);

        return updatedFeedbackDTO;
    }
}
