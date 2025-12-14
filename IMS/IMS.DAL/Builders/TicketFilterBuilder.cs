using IMS.DAL.Entities;
using Shared.Enums;

namespace IMS.DAL.Builders;

public interface ITicketFilterBuilder
{
    ITicketFilterBuilder WithTitle(string? title);
    ITicketFilterBuilder WithDescription(string? description);
    ITicketFilterBuilder WithStatus(TicketStatus? status);
    ITicketFilterBuilder WithBoard(Guid? boardId);
    ITicketFilterBuilder WithDeadline(DateTime? deadline);
    IQueryable<Ticket> Build(IQueryable<Ticket> query);
}

public class TicketFilterBuilder : ITicketFilterBuilder
{
    private string? _title;
    private string? _description;
    private TicketStatus? _status;
    private Guid? _boardId;
    private DateTime? _deadline;

    public IQueryable<Ticket> Build(IQueryable<Ticket> query)
    {
        if(!string.IsNullOrEmpty(_title)) 
            query = query.Where(t => t.Title == _title);

        if(!string.IsNullOrEmpty(_description))
            query = query.Where(t => t.Description == _description);

        if(_status.HasValue) 
            query = query.Where(t => t.Status == _status.Value);

        if(_boardId.HasValue) 
            query = query.Where(t => t.BoardId == _boardId.Value);

        if(_deadline.HasValue)
            query = query.Where(t => t.DeadLine == _deadline.Value);

        return  query;
    }

    public ITicketFilterBuilder WithBoard(Guid? boardId)
    {
        _boardId = boardId;
        return this;
    }

    public ITicketFilterBuilder WithStatus(TicketStatus? status)
    {
        _status = status;
        return this;
    }

    public ITicketFilterBuilder WithTitle(string? title)
    {
        _title = title;
        return this;
    }

    public ITicketFilterBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public ITicketFilterBuilder WithDeadline(DateTime? deadline)
    {
        _deadline = deadline;
        return this;
    }
}
