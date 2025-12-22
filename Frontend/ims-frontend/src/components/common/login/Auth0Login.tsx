import React, { useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import styles from './Auth0Login.module.css';
import {FAQIcon, FeedbackIcon, HelpIcon, LogoutIcon, PersonIcon, SettingsIcon} from "../Icons.tsx";
import {useUserRole} from "../../../hooks/useUserRole.ts";

const Auth0Login: React.FC = () => {
  const { logout, user, isAuthenticated, isLoading } = useAuth0();
  const [isMenuOpen, setIsMenuOpen] = useState(false); 
  const [isDarkMode, setIsDarkMode] = useState(true);
  const {role, isLoadingRole} = useUserRole();

  if (isLoading || isLoadingRole) {
    return null; 
  }

  if (!isAuthenticated) {
      return null;
  }

  const userTitle = role === "HRManager" ? "HR Manager" : role;

  const handleProfileClick = () => {
    setIsMenuOpen(prev => !prev);
  };
  
  const handleLogout = () => {
    setIsMenuOpen(false);
    logout({ logoutParams: { returnTo: globalThis.location.origin } });
  };

  const handleToggleTheme = (e: React.MouseEvent) => {
      e.stopPropagation();
      setIsDarkMode(prev => !prev);
  }

  return (
    <div className={styles.profileContainer} onClick={handleProfileClick}>
        
        {user?.picture && (
            <img 
                src={user.picture} 
                alt={user.name || 'User Avatar'} 
                className={styles.avatar}
            />
        )}
        
        <div className={styles.userInfo}>
            <div className={styles.userName}>
                {user?.name || 'User'}
            </div>
            <div className={styles.userRole}>
                {userTitle} 
            </div>
        </div>
        
        <svg 
            className={styles.dropdownArrow} 
            style={{ transform: isMenuOpen ? 'rotate(180deg)' : 'rotate(0deg)' }}
            viewBox="0 0 24 24" 
            fill="none" 
            xmlns="http://www.w3.org/2000/svg"
        >
            <path d="M7 10L12 15L17 10H7Z" fill="currentColor"/>
        </svg>

        {isMenuOpen && (
            <div className={styles.dropdownMenu} onClick={(e) => e.stopPropagation()}>
                
                <div className={styles.menuItem}>
                    <PersonIcon /> Profile
                </div>
                
                <div className={styles.menuItem}>
                    <SettingsIcon /> Settings
                </div>

                <div className={styles.separator} />

                <div className={`${styles.menuItem} ${styles.settingsItem} ${styles.disabled}`}>
                    <span className={styles.settingsLabel}>Language</span>
                    <span className={styles.languageSelector}>English <svg width="12" height="12" viewBox="0 0 24 24" fill="none"><path fill="currentColor" d="M7 10L12 15L17 10H7Z"/></svg></span>
                </div>
                
                <div className={`${styles.menuItem} ${styles.settingsItem} ${styles.disabled}`}>
                    <span className={styles.settingsLabel}>Dark Theme</span>
                    <div 
                        className={`${styles.toggleSwitch} ${isDarkMode ? styles.active : ''}`}
                        onClick={handleToggleTheme}
                    >
                        <div className={styles.toggleHandle}></div>
                    </div>
                </div>

                <div className={styles.separator} />
                
                <div className={styles.menuItem}>
                    <HelpIcon /> Technical Support
                </div>

                <div className={styles.menuItem}>
                    <FAQIcon /> FAQ
                </div>

                <div className={styles.menuItem}>
                    <FeedbackIcon /> Leave Feedback
                </div>

                <div className={styles.separator} />
                
                <div
                    className={`${styles.menuItem} ${styles.logoutButton}`} 
                    onClick={handleLogout}
                >
                    <LogoutIcon /> Log Out
                </div>
            </div>
        )}
    </div>
  );
};

export default Auth0Login;
