namespace IMS.NotificationsCore.Messages;

public record UserCreatedEvent( 
    string FirstName, 
    string LastName, 
    string Role,
    string Email) 
    : BaseEvent();
