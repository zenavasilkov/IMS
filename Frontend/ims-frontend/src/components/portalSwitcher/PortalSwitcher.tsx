import React from 'react';
import styles from './PortalSwitcher.module.css';

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
    userRole: string | null;
}

const PortalSwitcher: React.FC<PortalSwitcherProps> = ({ activePortal, onSwitch, userRole }) => {

    const canAccessManagement = userRole === 'HRManager';

    if (!canAccessManagement) {
        return null;
    }

    return (
        <div className={styles.portalSwitcherContainer}>

            <button
                onClick={() => onSwitch(PORTALS.INTERVIEWS)}
                className={`${styles.portalButton} ${activePortal === PORTALS.INTERVIEWS ? styles.active : ''}`}
                type="button"
            >
                Interviews
            </button>

            <button
                onClick={() => onSwitch(PORTALS.DEPARTMENTS)}
                className={`${styles.portalButton} ${activePortal === PORTALS.DEPARTMENTS ? styles.active : ''}`}
                type="button"
            >
                Departments
            </button>

            <button
                onClick={() => onSwitch(PORTALS.EMPLOYEES)}
                className={`${styles.portalButton} ${activePortal === PORTALS.EMPLOYEES ? styles.active : ''}`}
                type="button"
            >
                Employees
            </button>

            <button
                onClick={() => onSwitch(PORTALS.RECRUITMENT)}
                className={`${styles.portalButton} ${activePortal === PORTALS.RECRUITMENT ? styles.active : ''}`}
                type="button"
            >
                Recruitment
            </button>

            <button
                onClick={() => onSwitch(PORTALS.USER_MANAGEMENT)}
                className={`${styles.portalButton} ${activePortal === PORTALS.USER_MANAGEMENT ? styles.active : ''}`}
                type="button"
            >
                User Management
            </button>

        </div>
    );
};

export default PortalSwitcher;
