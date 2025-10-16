using AutoMapper;
using IMS.BLL.Models;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;

namespace IMS.BLL.Services;

public class FeedbackService(IFeedbackRepository repository, IMapper mapper)
    : Service<FeedbackModel, Feedback>(repository, mapper)
{
    private readonly IMapper _mapper = mapper;

    public override async Task<FeedbackModel> UpdateAsync(Guid id, FeedbackModel model, CancellationToken cancellationToken = default)
    {
        var existingFeedback = await repository.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new Exception($"Feedback with ID {id} was not found");

        existingFeedback.Comment = model.Comment;

        var updatedFeedback = await repository.UpdateAsync(existingFeedback, cancellationToken: cancellationToken);

        var updatedFeedbackModel = _mapper.Map<FeedbackModel>(updatedFeedback);

        return updatedFeedbackModel;
    }
}
