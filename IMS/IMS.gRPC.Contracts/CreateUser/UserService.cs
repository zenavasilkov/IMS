using MagicOnion;
using MessagePack;

namespace IMS.gRPC.Contracts.CreateUser;

public interface IUserService : IService<IUserService>
{
    UnaryResult<CreateUserResponse> CreateUser(CreateUserRequest request);
}

[MessagePackObject]
public class CreateUserRequest
{
    [Key(0)] public string Email { get; set; } = "";
    [Key(1)] public string PhoneNumber { get; set; } = "";
    [Key(2)] public string FirstName { get; set; } = "";
    [Key(3)] public string LastName { get; set; } = "";
    [Key(4)] public string? Patronymic { get; set; }
}

[MessagePackObject]
public record CreateUserResponse
{
    [Key(0)] public Guid Id { get; set; }
}
