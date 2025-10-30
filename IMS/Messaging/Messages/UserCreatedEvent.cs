namespace IMS.Messaging.Messaging;

public record UserCreatedEvent( 
    string FirstName, 
    string LastName, 
    string Role,
    string Email) 
    : BaseEvent();
