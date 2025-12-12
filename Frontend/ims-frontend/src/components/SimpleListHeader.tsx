import React from 'react';
import commonStyles from './common/commonStyles/commonPageStyles.module.css';

interface SimpleListHeaderProps {
    columns: { label: string; flex: number; textAlign?: 'left' | 'center' | 'right' }[];
}

const SimpleListHeader: React.FC<SimpleListHeaderProps> = ({ columns }) => {
    return (
        <div className={commonStyles.listHeader}>
            {columns.map((col, index) => (
                <span key={index} style={{ flex: col.flex, textAlign: col.textAlign || 'left' }}>
                    {col.label}
                </span>
            ))}
        </div>
    );
};

export default SimpleListHeader;