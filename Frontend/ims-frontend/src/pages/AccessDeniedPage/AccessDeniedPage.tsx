import React from 'react';
import styles from './AccessDeniedPage.module.css';
import {ForbiddenIcon} from "../../components/common/Icons.tsx";

const AccessDeniedPage: React.FC = () => {
    return (
        <div className={styles.denialContainer}>
            <div className={styles.denialCard}>
                <ForbiddenIcon className={styles.denialIcon} />
                <h1 className={styles.denialTitle}>Access Denied</h1>
                <p className={styles.denialMessage}>You do not have the required permissions (Role) to view the requested portal or page.</p>
                <p className={styles.denialDetail}>Please contact your system administrator if you believe this is an error.</p>
            </div>
        </div>
    );
};

export default AccessDeniedPage;
