using Shared.Enums;

namespace IMS.Presentation.Routing;

public static class ApiRoutes
{
    public const string Id = "{id:guid}";

    private const string api = "api";

    public static class Boards 
    {
        public const string Base = $"{api}/boards";
        public const string AddTicket = "{boardId:guid}/tickets/{ticketId:guid}";
    }

    public static class Feedbacks
    {
        public const string Base = $"{api}/feedbacks";
    }

    public static class Internships
    {
        public const string Base = $"{api}/internships";
    } 

    public static class Tickets
    {
        public const string Base = $"{api}/tickets";
    }

    public static class Users
    {
        public const string Base = $"{api}/users";
        public const string AddInternToMentor = "mentor/{mentorId:guid}/intern/{internId:guid}";
        public const string Role = "Role/{Role:role}";
    }
}
