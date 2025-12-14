using IMS.DAL.Entities;

namespace IMS.DAL.Builders;

public interface IFeedbackFilterBuilder
{
    IFeedbackFilterBuilder WithTicket(Guid? ticketId);
    IFeedbackFilterBuilder SentBy(Guid? sentById);
    IFeedbackFilterBuilder SentTo(Guid? sentToId);
    IFeedbackFilterBuilder WithComment(string? comment);
    IQueryable<Feedback> Build(IQueryable<Feedback> query);
}

public class FeedbackFilterBuilder : IFeedbackFilterBuilder
{
    private Guid? _ticketId;
    private Guid? _sentById;
    private Guid? _sentToId;
    private string? _comment;

    public IQueryable<Feedback> Build(IQueryable<Feedback> query)
    {
        if (_ticketId.HasValue)
            query = query.Where(f => f.TicketId == _ticketId);

        if(_sentById.HasValue)
            query = query.Where(f => f.SentById == _sentById);

        if(_sentToId.HasValue)
            query = query.Where(f => f.AddressedToId == _sentToId);

        if(!string.IsNullOrEmpty(_comment))
            query = query.Where(f => f.Comment == _comment);

        return query;
    }

    public IFeedbackFilterBuilder WithTicket(Guid? ticketId)
    {
        _ticketId = ticketId;
        return this;
    }

    public IFeedbackFilterBuilder SentBy(Guid? sentById)
    {
        _sentById = sentById;
        return this;
    }

    public IFeedbackFilterBuilder SentTo(Guid? sentToId)
    {
        _sentToId = sentToId;
        return this;
    }

    public IFeedbackFilterBuilder WithComment(string? comment)
    {
        _comment = comment;
        return this;
    }
}
