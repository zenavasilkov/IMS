import React from 'react';
import styles from './PortalSwitcher.module.css';

interface PortalButtonProps {
    portalName: string;
    label: string;
    activePortal: string;
    onSwitch: (portal: string) => void;
}

const PortalButton: React.FC<PortalButtonProps> = ({ portalName, label, activePortal, onSwitch }) => {

    const isActive = activePortal === portalName;

    return (
        <button
            onClick={() => onSwitch(portalName)}
            className={`${styles.portalButton} ${isActive ? styles.active : ''}`}
            type="button"
        >
            {label}
        </button>
    );
};

export default PortalButton;
