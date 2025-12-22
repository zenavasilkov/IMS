namespace IMS.Presentation.Routing;

public static class ApiRoutes
{
    public const string Id = "{id:guid}";

    private const string Api = "api";

    public static class Boards 
    {
        public const string Base = $"{Api}/boards";
        public const string AddTicket = "{boardId:guid}/tickets/{ticketId:guid}";
    }

    public static class Feedbacks
    {
        public const string Base = $"{Api}/feedbacks";
        public const string FeedbacksByTicketId = "by-ticket/{ticketId:guid}";
    }

    public static class Internships
    {
        public const string Base = $"{Api}/internships";
        public const string InternshipsByMentorId = "by-mentor/{mentorId:guid}";
    } 

    public static class Tickets
    {
        public const string Base = $"{Api}/tickets";
        public const string TicketsByBoardId = "by-board/{boardId:guid}";
    }

    public static class Users
    {
        public const string Base = $"{Api}/users";
        public const string AddInternToMentor = "mentor/{mentorId:guid}/intern/{internId:guid}";
        public const string UsersByRole = "{role}";
    }
}
