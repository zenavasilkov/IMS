import React from 'react';
import styles from './common/commonStyles/commonModalStyles.module.css';

interface ModalWrapperProps {
    title: string;
    isOpen: boolean;
    onClose: () => void;
    error: string | null;
    children: React.ReactNode;
}

const ModalWrapper: React.FC<ModalWrapperProps> = ({ title, isOpen, onClose, error, children }) => {

    if (!isOpen) return null;

    return (
        <div className={styles.modalOverlay}>
            <div className={styles.modalContent}>
                <h2 className={styles.modalTitle}>{title}</h2>

                <button className={styles.closeButton} onClick={onClose}>&times;</button>

                {error && <div className={styles.error}>{error}</div>}

                {children}
            </div>
        </div>
    );
};

export default ModalWrapper;
