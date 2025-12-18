import React from 'react';
import styles from './PortalSwitcher.module.css';
import PortalButton from "./PortalButton.tsx";

export const PORTALS = {
    RECRUITMENT: 'Recruitment',
    USER_MANAGEMENT: 'UserManagement',
    EMPLOYEES: 'Employees',
    DEPARTMENTS: 'Departments',
    INTERVIEWS: 'Interviews',
    INTERNSHIPS: 'Internships',
    MENTOR_INTERNS: 'MentorInterns',
    BOARD_VIEW: 'BoardView',
};

interface PortalSwitcherProps {
    activePortal: string;
    onSwitch: (portal: string) => void;
    role: string | null;
}

const PortalSwitcher: React.FC<PortalSwitcherProps> = ({ activePortal, onSwitch, role }) => {
    const visibleToHrManager = role === "HRManager";
    const visibleToMentor = role === "Mentor";

    return (
        <div className={styles.portalSwitcherContainer}>

            { visibleToHrManager && <PortalButton portalName={PORTALS.INTERVIEWS} label="Interviews" activePortal={activePortal} onSwitch={onSwitch} />}

            { visibleToHrManager && <PortalButton portalName={PORTALS.DEPARTMENTS} label="Departments" activePortal={activePortal} onSwitch={onSwitch} />}

            { visibleToHrManager && <PortalButton portalName={PORTALS.EMPLOYEES} label="Employees" activePortal={activePortal} onSwitch={onSwitch} />}

            { visibleToHrManager && <PortalButton portalName={PORTALS.RECRUITMENT} label="Recruitment" activePortal={activePortal} onSwitch={onSwitch} />}

            { visibleToHrManager && <PortalButton portalName={PORTALS.INTERNSHIPS} label="Internships" activePortal={activePortal} onSwitch={onSwitch} />}

            { visibleToHrManager && <PortalButton portalName={PORTALS.USER_MANAGEMENT} label="User Management" activePortal={activePortal} onSwitch={onSwitch} />}

            { visibleToMentor && <PortalButton portalName={PORTALS.MENTOR_INTERNS} label="My Interns" activePortal={activePortal} onSwitch={onSwitch} />}

        </div>
    );
};

export default PortalSwitcher;
