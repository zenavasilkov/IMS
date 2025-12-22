using IMS.DAL.Entities;
using Shared.Enums;

namespace IMS.DAL.Builders;

public interface IUserFilterBuilder
{
    IUserFilterBuilder WithEmail(string? email);
    IUserFilterBuilder WithPhoneNumber(string? phoneNumber);
    IUserFilterBuilder WithFirstName(string? firstname);
    IUserFilterBuilder WithLastName(string? lastname);
    IUserFilterBuilder WithPatronymic(string? patronymic);
    IUserFilterBuilder WithRole(Role? role);
    IQueryable<User> Build(IQueryable<User> query);
}

public class UserFilterBuilder : IUserFilterBuilder
{
    private string? _email;
    private string? _phoneNumber;
    private string? _firstname;
    private string? _lastname;
    private string? _patronymic;
    private Role? _role;

    public IUserFilterBuilder WithEmail(string? email)
    {
        _email = email;
        return this;
    }

    public IUserFilterBuilder WithPhoneNumber(string? phoneNumber)
    {
        _phoneNumber = phoneNumber;
        return this;
    }

    public IUserFilterBuilder WithFirstName(string? firstname)
    {
        _firstname = firstname;
        return this;
    }

    public IUserFilterBuilder WithLastName(string? lastname)
    {
        _lastname = lastname;
        return this;
    }

    public IUserFilterBuilder WithPatronymic(string? patronymic)
    {
        _patronymic = patronymic;
        return this;
    }

    public IUserFilterBuilder WithRole(Role? role)
    {
        _role = role;
        return this;
    }

    public IQueryable<User> Build(IQueryable<User> query)
    {
        if (!string.IsNullOrWhiteSpace(_email))
            query = query.Where(u => u.Email.Contains(_email));

        if (!string.IsNullOrWhiteSpace(_phoneNumber))
            query = query.Where(u => u.PhoneNumber.Contains(_phoneNumber));

        if (!string.IsNullOrWhiteSpace(_firstname))
            query = query.Where(u => u.Firstname.Contains(_firstname));

        if (!string.IsNullOrWhiteSpace(_lastname))
            query = query.Where(u => u.Lastname.Contains(_lastname));

        if (!string.IsNullOrWhiteSpace(_patronymic))
            query = query.Where(u => u.Patronymic != null && u.Patronymic.Contains(_patronymic));

        if (_role.HasValue)
            query = query.Where(u => u.Role == _role.Value);

        return query;
    }
}
