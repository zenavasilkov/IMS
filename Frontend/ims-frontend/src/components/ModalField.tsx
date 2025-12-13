import React from 'react';
import styles from './common/commonStyles/commonModalStyles.module.css';

interface ModalFieldProps extends React.InputHTMLAttributes<HTMLInputElement> {
    label: string;
}

const ModalField: React.FC<ModalFieldProps> = ({ label, children, ...props }) => {

    return (
        <label>
            {label}
            <input
                {...props}
                className={styles.formInput}
            />
        </label>
    );
};

export default ModalField;
