using AutoMapper;
using IMS.BLL.Exceptions;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;

namespace IMS.BLL.Services;

public class FeedbackService(IFeedbackRepository repository, IMapper mapper)
    : Service<FeedbackModel, Feedback>(repository, mapper), IFeedbackService
{
    private readonly IMapper _mapper = mapper;

    public async Task<List<FeedbackModel>> GetFeedbacksByTicketIdAsync(Guid feedbackId, 
        bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var feedbacks = await GetAllAsync(f => f.TicketId ==  feedbackId, cancellationToken : cancellationToken);

        return feedbacks;
    }

    public override async Task<FeedbackModel> UpdateAsync(Guid id, FeedbackModel model, CancellationToken cancellationToken = default)
    {
        var existingFeedback = await repository.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Feedback with ID {id} was not found");

        existingFeedback.Comment = model.Comment;

        var updatedFeedback = await repository.UpdateAsync(existingFeedback, cancellationToken: cancellationToken);

        var updatedFeedbackModel = _mapper.Map<FeedbackModel>(updatedFeedback);

        return updatedFeedbackModel;
    }
}
