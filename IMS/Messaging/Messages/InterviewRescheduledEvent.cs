using IMS.NotificationsCore.Messages;

namespace RecruitmentNotifications.Messages;

public record InterviewRescheduledEvent(
    string CandidateEmail,
    string InterviewerEmail,
    string InterviewType,
    DateTime ScheduledAt,
    DateTime RescheduledTo) : BaseEvent();
