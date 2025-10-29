using AutoMapper;
using IMS.BLL.Exceptions;
using IMS.BLL.Logging;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace IMS.BLL.Services;

public class FeedbackService(IFeedbackRepository repository, ITicketRepository ticketRepository,
    IUserRepository userRepository, IMapper mapper, ILogger<FeedbackService> logger)
    : Service<FeedbackModel, Feedback>(repository, mapper, logger), IFeedbackService
{
    private readonly IMapper _mapper = mapper;

    public async Task<List<FeedbackModel>> GetFeedbacksByTicketIdAsync(Guid feedbackId,
        bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var feedbacks = await repository.GetAllAsync(f => f.TicketId == feedbackId, false, cancellationToken);

        var feedbackModels = _mapper.Map<List<FeedbackModel>>(feedbacks);

        return feedbackModels;
    }

    public override async Task<FeedbackModel> CreateAsync(FeedbackModel feedback, CancellationToken cancellationToken = default)
    {
        if (await ticketRepository.GetByIdAsync(feedback.TicketId, cancellationToken: cancellationToken) is null)
            throw new NotFoundException($"Ticket with ID {feedback.TicketId} was not found");

        if (await userRepository.GetByIdAsync(feedback.SentById, cancellationToken: cancellationToken) is null)
            throw new NotFoundException($"User with ID {feedback.SentById} was not found");

        if (await userRepository.GetByIdAsync(feedback.AddressedToId, cancellationToken: cancellationToken) is null)
            throw new NotFoundException($"User with ID {feedback.AddressedToId} was not found");

        var feedbackModel = await base.CreateAsync(feedback, cancellationToken);

        logger.LogInformation(LoggingConstants.RESOURCE_CREATED, nameof(Ticket), feedbackModel.Id);

        return feedbackModel;
    }

    public override async Task<FeedbackModel> UpdateAsync(Guid id, FeedbackModel model, CancellationToken cancellationToken = default)
    {
        var existingFeedback = await repository.GetByIdAsync(id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"Feedback with ID {id} was not found");

        existingFeedback.Comment = model.Comment;

        var updatedFeedback = await repository.UpdateAsync(existingFeedback, cancellationToken: cancellationToken);

        logger.LogInformation(LoggingConstants.RESOURCE_UPDATED, nameof(Ticket), id);

        var updatedFeedbackModel = _mapper.Map<FeedbackModel>(updatedFeedback);

        return updatedFeedbackModel;
    }
}
