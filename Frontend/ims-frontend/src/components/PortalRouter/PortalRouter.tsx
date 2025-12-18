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


const getPortalContent = ({ role, activePortal }: PortalRouterProps): RouteResult => {

    let content: React.ReactNode;
    let header: React.ReactNode;

    const hrManager = 'HRManager';
    const mentor = 'Mentor';

    if (role === hrManager) {
        if (activePortal === PORTALS.RECRUITMENT) {
            content = <RecruitmentPage />;
            header = HrManagerHeader("Recruitment Management");
        } else if (activePortal === PORTALS.INTERVIEWS) {
            content = <InterviewPage />;
            header = HrManagerHeader("Interview Management");
        } else if (activePortal === PORTALS.USER_MANAGEMENT) {
            content = <UserManagementPage />;
            header = HrManagerHeader("User Management");
        } else if (activePortal === PORTALS.DEPARTMENTS) {
            content = <DepartmentPage />;
            header = HrManagerHeader("Department Management");
        } else if (activePortal === PORTALS.EMPLOYEES) {
            content = <EmployeePage />;
            header = HrManagerHeader("Employee Management");
        } else if (activePortal === PORTALS.INTERNSHIPS) {
            content = <InternshipPage/>;
            header = HrManagerHeader("Internship Management");
        } else {
            content = <RecruitmentPage />;
            header = HrManagerHeader("Recruitment Management");
        }
    }

    else if (role === mentor){
        if (activePortal === PORTALS.MENTOR_INTERNS) {
            content = <MentorPage />;
            header = MentorHeader("Intern Management");
        }
        else if (activePortal === PORTALS.BOARD_VIEW) {
            content = <BoardPage />;
            header = MentorHeader("Kanban Board");
        } else {
            content = <MentorPage />;
            header = MentorHeader("Intern Management");
        }
    }

    else {
        content = <div className="No-Access">Access Denied: Your role does not allow portal switching.</div>;
        header = "IMS Portal";
    }

    return { content, headerTitle: header };
};

export default getPortalContent;
