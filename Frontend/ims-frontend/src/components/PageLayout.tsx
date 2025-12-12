import React from 'react';
import commonStyles from './common/commonStyles/commonPageStyles.module.css';

interface PageLayoutProps {
    title: string;
    Icon: React.FC<any>;
    children: React.ReactNode;
    createButton: React.ReactNode | null;
    iconColor: string;
}

const PageLayout: React.FC<PageLayoutProps> = ({ title, Icon, children, createButton, iconColor }) => {
    return (
        <div className={commonStyles.container}>

            <div className={commonStyles.titleArea}>
                <h1 className={commonStyles.heading}>
                    <Icon className={commonStyles.headingIcon} style={{ color: iconColor }}/>
                    {title}
                </h1>
                {createButton}
            </div>

            <div className={commonStyles.contentWrapper}>
                {children}
            </div>
        </div>
    );
};

export default PageLayout;
