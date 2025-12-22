namespace IMS.DAL.Outbox;

public record OutboxUserRoleUpdate(Guid UserId, string Email, string NewRole);
