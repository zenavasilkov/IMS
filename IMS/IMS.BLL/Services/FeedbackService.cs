using AutoMapper;
using IMS.BLL.Exceptions;
using IMS.BLL.Mapping;
using IMS.BLL.Models;
using IMS.BLL.Services.Interfaces;
using IMS.DAL.Entities;
using IMS.DAL.Repositories.Interfaces;
using IMS.NotificationsCore.Services;
using Shared.Filters;
using Shared.Pagination;

namespace IMS.BLL.Services;

public class FeedbackService(
    IFeedbackRepository repository, 
    ITicketRepository ticketRepository,
    IUserRepository userRepository, 
    IMapper mapper,
    IMessageService messageService)
    : Service<FeedbackModel, Feedback>(repository, mapper), IFeedbackService
{
    private readonly IMapper _mapper = mapper;

    public async Task<List<FeedbackModel>> GetFeedbacksByTicketIdAsync(Guid id,
        bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        var feedbacks = await repository.GetAllAsync(f => f.TicketId == id, false, cancellationToken);

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

        var message = EventMapper.ConvertToFeedbackCreatedEvent(feedbackModel);

        await messageService.NotifyFeedbackCreated(message, cancellationToken);

        return feedbackModel;
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

    public async Task<PagedList<FeedbackModel>> GetAllAsync(
        PaginationParameters paginationParameters,
        FeedbackFilteringParameters filter,
        bool trackChanges,
        CancellationToken cancellationToken)
    {
        var feedbacks = await repository.GetAllAsync(paginationParameters, filter, trackChanges, cancellationToken);
        
        var feedbackModels = _mapper.Map<PagedList<FeedbackModel>>(feedbacks);
        
        return feedbackModels;
    }
}
