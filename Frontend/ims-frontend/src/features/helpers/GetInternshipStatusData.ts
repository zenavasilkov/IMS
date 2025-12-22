import {InternshipStatus} from "../../entities/ims/enums.ts";

export const getStatusData = (status: InternshipStatus) => {
    const text = InternshipStatus[status].replaceAll(/([A-Z])/g, ' $1').trim();
    let className = '';

    switch (status) {
        case InternshipStatus.NotStarted:
            className = 'statusNotStarted';
            break;
        case InternshipStatus.Ongoing:
            className = 'statusOngoing';
            break;
        case InternshipStatus.Completed:
            className = 'statusCompleted';
            break;
        case InternshipStatus.Cancelled:
            className = 'statusCancelled';
            break;
        default:
    }
    return { text, className };
};
