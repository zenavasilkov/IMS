using AutoMapper;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.Presentation.DTOs.CreateDTO;
using IMS.Presentation.DTOs.GetDTO;
using IMS.Presentation.DTOs.UpdateDTO;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedbacksController(IFeedbackService service, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FeedbackDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var feedbacks = await service.GetAllAsync(null, false, cancellationToken);

        if (feedbacks.Count == 0) return NoContent();

        var feedbackDTOs = mapper.Map<IEnumerable<FeedbackDTO>>(feedbacks);
         
        return Ok(feedbackDTOs);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FeedbackDTO>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var feedback = await service.GetByIdAsync(id, cancellationToken);

        if (feedback is null) return NotFound(new { message = $"Feedback with ID {id} was not found." });

        var feedbackDTO = mapper.Map<FeedbackDTO>(feedback);

        return Ok(feedbackDTO);
    }

    [HttpPost]
    public async Task<ActionResult<FeedbackDTO>> Create([FromBody] CreateFeedbackDTO createFeedbackDTO, CancellationToken cancellationToken)
    {
        var feedbackModel = mapper.Map<FeedbackModel>(createFeedbackDTO);

        var createdFeedbackModel = await service.CreateAsync(feedbackModel, cancellationToken);

        var createdFeedbackDTO = mapper.Map<FeedbackDTO>(createdFeedbackModel);

        return CreatedAtAction(nameof(GetById), new { id = createdFeedbackDTO.Id }, createdFeedbackDTO);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<FeedbackDTO>> Update(Guid id, [FromBody] UpdateFeedbackDTO updateFeedbackDTO, CancellationToken cancellationToken)
    {
        var feedbackModel = mapper.Map<FeedbackModel>(updateFeedbackDTO);

        feedbackModel.Id = id;

        var updatedFeedbackModel = await service.UpdateAsync(feedbackModel, cancellationToken);

        if (updatedFeedbackModel is null) return NotFound(new { message = $"Feedback with ID {id} was not found." });

        var updatedFeedbackDTO = mapper.Map<FeedbackDTO>(updatedFeedbackModel);

        return Ok(updatedFeedbackDTO);
    }
}
