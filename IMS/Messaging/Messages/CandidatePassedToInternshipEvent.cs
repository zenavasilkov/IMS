using IMS.NotificationsCore.Messages;

namespace RecruitmentNotifications.Messages;

public record CandidatePassedToInternshipEvent(string Email) : BaseEvent();
