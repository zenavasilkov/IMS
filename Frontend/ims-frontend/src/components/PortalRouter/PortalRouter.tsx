import {ManagementIcon, MentorIcon} from "../common/Icons.tsx";
import {PORTALS} from "../portalSwitcher/PortalSwitcher.tsx";
import RecruitmentPage from "../../pages/RecruitmentManagementPage/RecruitmentPage.tsx";
import InterviewPage from "../../pages/InterviewPage/InterviewPage.tsx";
import UserManagementPage from "../../pages/UserManagementPage/UserManagementPage.tsx";
import DepartmentPage from "../../pages/DepartmentPage/DepartmentPage.tsx";
import EmployeePage from "../../pages/EmployeePage/EmployeePage.tsx";
import InternshipPage from "../../pages/InternshipPage/InternshipPage.tsx";
import BoardPage from "../../pages/BoardPage/BoardPage.tsx";
import MentorPage from "../../pages/MentorPage/MentorPage.tsx";
import React from "react";
import AccessDeniedPage from "../../pages/AccessDeniedPage/AccessDeniedPage.tsx";

const HrManagerHeader = (text : string) => <div><ManagementIcon className="App-Header-Icon" />{text}</div>;
const MentorHeader = (text : string) => <div><MentorIcon className="App-Header-Icon" />{text}</div>;

interface RouteResult {
    content: React.ReactNode;
    headerTitle: React.ReactNode;
}

interface PortalRouterProps {
    role: string | null;
    activePortal: string;
}

const HR_MANAGER_ROUTES: Record<string, RouteResult> = {
    [PORTALS.RECRUITMENT]: { content: <RecruitmentPage />, headerTitle: HrManagerHeader("Recruitment Management") },
    [PORTALS.INTERVIEWS]: { content: <InterviewPage />, headerTitle: HrManagerHeader("Interview Management") },
    [PORTALS.USER_MANAGEMENT]: { content: <UserManagementPage />, headerTitle: HrManagerHeader("User Management") },
    [PORTALS.DEPARTMENTS]: { content: <DepartmentPage />, headerTitle: HrManagerHeader("Department Management") },
    [PORTALS.EMPLOYEES]: { content: <EmployeePage />, headerTitle: HrManagerHeader("Employee Management") },
    [PORTALS.INTERNSHIPS]: { content: <InternshipPage/>, headerTitle: HrManagerHeader("Internship Management") },
};

const MENTOR_ROUTES: Record<string, RouteResult> = {
    [PORTALS.MENTOR_INTERNS]: { content: <MentorPage />, headerTitle: MentorHeader("Intern Management") },
    [PORTALS.BOARD_VIEW]: { content: <BoardPage />, headerTitle: MentorHeader("Kanban Board") },
};

const INTERN_ROUTES: Record<string, RouteResult> = {
    [PORTALS.BOARD_VIEW]: { content: <BoardPage />, headerTitle: MentorHeader("My Kanban Board") },
};

const getPortalContent = ({ role, activePortal }: PortalRouterProps): RouteResult => {
    const hrManager = 'HRManager';
    const mentor = 'Mentor';
    const intern = 'Intern';

    if (role === hrManager) {
        const result = HR_MANAGER_ROUTES[activePortal];

        if (result) {
            return result;
        } else {
            return HR_MANAGER_ROUTES[PORTALS.RECRUITMENT];
        }
    }
    else if (role === mentor) {
        const result = MENTOR_ROUTES[activePortal];

        if (result) {
            return result;
        } else {
            return MENTOR_ROUTES[PORTALS.MENTOR_INTERNS];
        }
    }
    else if (role === intern) {
        return INTERN_ROUTES[PORTALS.BOARD_VIEW];
    }
    else {
        return {
            content: <AccessDeniedPage />,
            headerTitle: "IMS Portal"
        };
    }
};

export default getPortalContent;
