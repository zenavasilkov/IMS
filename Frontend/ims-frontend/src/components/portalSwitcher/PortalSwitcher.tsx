import React from 'react';
import styles from './PortalSwitcher.module.css';
import PortalButton from "./PortalButton.tsx";

export const PORTALS = {
    RECRUITMENT: 'Recruitment',
    USER_MANAGEMENT: 'UserManagement',
    EMPLOYEES: 'Employees',
    DEPARTMENTS: 'Departments',
    INTERVIEWS: 'Interviews',
};

interface PortalSwitcherProps {
    activePortal: string;
    onSwitch: (portal: string) => void;
    isVisible: boolean;
}

const PortalSwitcher: React.FC<PortalSwitcherProps> = ({ activePortal, onSwitch, isVisible }) => {

    if (!isVisible) {
        return null;
    }

    return (
        <div className={styles.portalSwitcherContainer}>

            <PortalButton portalName={PORTALS.INTERVIEWS} label="Interviews" activePortal={activePortal} onSwitch={onSwitch} />

            <PortalButton portalName={PORTALS.DEPARTMENTS} label="Departments" activePortal={activePortal} onSwitch={onSwitch} />

            <PortalButton portalName={PORTALS.EMPLOYEES} label="Employees" activePortal={activePortal} onSwitch={onSwitch} />

            <PortalButton portalName={PORTALS.RECRUITMENT} label="Recruitment" activePortal={activePortal} onSwitch={onSwitch} />

            <PortalButton portalName={PORTALS.USER_MANAGEMENT} label="User Management" activePortal={activePortal} onSwitch={onSwitch} />

        </div>
    );
};

export default PortalSwitcher;
