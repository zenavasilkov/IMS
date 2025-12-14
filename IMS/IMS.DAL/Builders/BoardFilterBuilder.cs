using IMS.DAL.Entities;

namespace IMS.DAL.Builders;

public interface IBoardFilterBuilder
{
    IBoardFilterBuilder WithTitle(string? title);
    IBoardFilterBuilder WithDescription(string? description);
    IBoardFilterBuilder CreatedBy(Guid? mentorId);
    IBoardFilterBuilder CreatedTo(Guid? internId);
    IQueryable<Board> Build(IQueryable<Board> query);
}

public class BoardFilterBuilder : IBoardFilterBuilder
{
    private string? _title;
    private string? _description;
    private Guid? _createdById;
    private Guid? _createdToId;

    public IQueryable<Board> Build(IQueryable<Board> query)
    {
        if (_title != null)
            query = query.Where(b => b.Title == _title);
        
        if (_description != null)
            query = query.Where(b => b.Description.Contains(_description));
        
        if (_createdById != null)
            query = query.Where(b => b.CreatedById == _createdById);
        
        if (_createdToId != null)
            query = query.Where(b => b.CreatedToId == _createdToId);
        
        return query;
    }
    
    public IBoardFilterBuilder WithTitle(string? title)
    {
        _title = title;
        return this;
    }

    public IBoardFilterBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public IBoardFilterBuilder CreatedBy(Guid? mentorId)
    {
        _createdById = mentorId;
        return this;
    }

    public IBoardFilterBuilder CreatedTo(Guid? internId)
    {
        _createdToId = internId;
        return this;
    }
}
