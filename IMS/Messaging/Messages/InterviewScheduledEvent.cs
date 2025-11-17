using IMS.NotificationsCore.Messages;

namespace RecruitmentNotifications.Messages;

public record InterviewScheduledEvent(
    string CandidateEmail,
    string InterviewerEmail,
    DateTime ScheduledAt,
    string InterviewType
    ) : BaseEvent();
