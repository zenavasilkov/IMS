export enum Role {
    HumanResourcesManager = 2,
    Mentor = 3,
    Intern = 4
}

export enum TicketStatus {
    Unassigned = 0,
    ToDo = 1,
    InProgress = 2,
    PullRequest = 3,
    Done = 4
}

export enum InternshipStatus{
    NotStarted = 0,
    Ongoing = 1,
    Completed = 2,
    Cancelled = 3
}

export enum UserSortingParameter{
    None = 0,
    AscendingId = 1,
    DescendingId = 2,
    AscendingFirstName = 3,
    DescendingFirstName = 4,
    AscendingLastName = 5,
    DescendingLastName = 6,
    AscendingRole = 7,
    DescendingRole = 8
}
