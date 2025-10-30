using System;
using System.Collections.Generic;
using System.Linq;
namespace IMS.Messaging.Messaging;

public record TicketCreatedEvent(
    string Title,
    string Description,
    DateTime Deadline,
    string Email) : BaseEvent;
