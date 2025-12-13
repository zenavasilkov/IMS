import React from 'react';
import styles from './common/commonStyles/commonModalStyles.module.css';

interface ModalSelectProps extends React.SelectHTMLAttributes<HTMLSelectElement> {
    label: string;
    options: { value: string; label: string }[];
}

const ModalSelect: React.FC<ModalSelectProps> = ({ label, options, ...props }) => {
    return (
        <label>
            {label}
            <select
                {...props}
                className={styles.formSelect}
            >
                {options.map(option => (
                    <option key={option.value} value={option.value}>
                        {option.label}
                    </option>
                ))}
            </select>
        </label>
    );
};

export default ModalSelect;
