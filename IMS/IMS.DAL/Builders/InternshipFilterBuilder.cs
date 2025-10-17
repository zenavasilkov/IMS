using IMS.DAL.Entities;
using Shared.Enums;

namespace IMS.DAL.Builders;

public interface IInternshipFilterBuilder
{
    IInternshipFilterBuilder WithIntern(Guid? internId);
    IInternshipFilterBuilder WithMentor(Guid? mentorId);
    IInternshipFilterBuilder WithHumanResourcesManager(Guid? HumanResourcesManagerId);
    IInternshipFilterBuilder StartingFrom(DateTime? startDate);
    IInternshipFilterBuilder EntingTo(DateTime? endDate);
    IInternshipFilterBuilder WithStatus(InternshipStatus? status);
    IQueryable<Internship> Build(IQueryable<Internship> query);
}

public class InternshipFilterBuilder : IInternshipFilterBuilder
{
    private Guid? _internId;
    private Guid? _mentorId;
    private Guid? _humanResourcesManagerId;
    private DateTime? _startDate;
    private DateTime? _endDate;
    private InternshipStatus? _status;

    public IQueryable<Internship> Build(IQueryable<Internship> query)
    {
        if(_internId.HasValue)
            query = query.Where(i => i.InternId == _internId.Value);

        if (_mentorId.HasValue)
            query = query.Where(i => i.MentorId == _mentorId.Value);

        if (_humanResourcesManagerId.HasValue)
            query = query.Where(i => i.InternId == _humanResourcesManagerId.Value);

        if (_startDate.HasValue)
            query = query.Where(i => i.StartDate >= _startDate);

        if(_endDate.HasValue)
            query = query.Where(i => i.EndDate >= _endDate);

        if (_status.HasValue)
            query = query.Where(i => i.Status == _status.Value);

        _internId = null;
        _mentorId = null;
        _humanResourcesManagerId = null;
        _startDate = null;
        _endDate = null;
        _status = null;

        return query;
    }

    public IInternshipFilterBuilder EntingTo(DateTime? endingDate)
    {
        _endDate = endingDate;
        return this;
    }

    public IInternshipFilterBuilder StartingFrom(DateTime? startDate)
    {
        _startDate = startDate;
        return this;
    }

    public IInternshipFilterBuilder WithHumanResourcesManager(Guid? HumanResourcesManagerId)
    {
        _humanResourcesManagerId = HumanResourcesManagerId;
        return this;
    }

    public IInternshipFilterBuilder WithIntern(Guid? internId)
    {
        _internId = internId;
        return this;
    }

    public IInternshipFilterBuilder WithMentor(Guid? mentorId)
    {
        _mentorId = mentorId;
        return this;
    }

    public IInternshipFilterBuilder WithStatus(InternshipStatus? status)
    {
        _status = status;
        return this;
    }
}
