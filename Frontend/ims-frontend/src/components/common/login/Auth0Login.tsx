import React, { useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import styles from './Auth0Login.module.css';

const PersonIcon = (props: any) => <svg {...props} viewBox="0 0 24 24" fill="none"><path fill="currentColor" d="M12 12c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm0 2c-2.67 0-8 1.34-8 4v2h16v-2c0-2.66-5.33-4-8-4z"/></svg>;
const SettingsIcon = (props: any) => <svg {...props} viewBox="0 0 24 24" fill="none"><path fill="currentColor" d="M19.4 12.99l.06-1.99a7.97 7.97 0 00-1.84-5.38l1.43-1.43a.99.99 0 000-1.4l-1.4-1.4a.99.99 0 00-1.4 0l-1.43 1.43a7.97 7.97 0 00-5.38-1.84L11 4.54a.99.99 0 00-1.99 0l-.06 1.99a7.97 7.97 0 00-5.38 1.84l-1.43-1.43a.99.99 0 00-1.4 0l-1.4 1.4a.99.99 0 000 1.4l1.43 1.43a7.97 7.97 0 00-1.84 5.38l-1.99.06a.99.99 0 000 1.99l1.99.06a7.97 7.97 0 001.84 5.38l-1.43 1.43a.99.99 0 000 1.4l1.4 1.4a.99.99 0 001.4 0l1.43-1.43a7.97 7.97 0 005.38 1.84l.06 1.99a.99.99 0 001.99 0l.06-1.99a7.97 7.97 0 005.38-1.84l1.43 1.43a.99.99 0 001.4 0l1.4-1.4a.99.99 0 000-1.4l-1.43-1.43a7.97 7.97 0 001.84-5.38l1.99-.06a.99.99 0 000-1.99l-1.99-.06zM12 16a4 4 0 110-8 4 4 0 010 8z"/></svg>;
const HelpIcon = (props: any) => <svg {...props} viewBox="0 0 24 24" fill="none"><path fill="currentColor" d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 17h-2v-2h2v2zm2.07-7.75l-.9.92C13.4 14.05 13 14.63 13 16h-2v-.75c0-1.63.6-3.13 1.57-4.05l.9-1.04c.73-.76 1.13-1.74 1.13-2.78 0-1.54-1.26-2.8-2.8-2.8s-2.8 1.26-2.8 2.8H7.2c0-2.43 1.83-4.4 4.14-4.8v-1.3H13v1.3c2.31.4 4.14 2.37 4.14 4.8 0 1.6-.66 3.1-1.76 4.3z"/></svg>;
const FAQIcon = (props: any) => <svg {...props} viewBox="0 0 24 24" fill="none"><path fill="currentColor" d="M16 11H8V9h8v2zm0 4H8v-2h8v2zm5-11H3v18h18V4zm-2 16H5V6h14v14z"/></svg>;
const FeedbackIcon = (props: any) => <svg {...props} viewBox="0 0 24 24" fill="none"><path fill="currentColor" d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/></svg>;
const LogoutIcon = (props: any) => <svg {...props} viewBox="0 0 24 24" fill="none"><path fill="currentColor" d="M17 7l-1.41 1.41L18.17 11H8v2h10.17l-2.58 2.59L17 17l5-5-5-5zM4 5h8V3H4c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h8v-2H4V5z"/></svg>;



const Auth0Login: React.FC = () => {
  const { logout, user, isAuthenticated, isLoading } = useAuth0();
  const [isMenuOpen, setIsMenuOpen] = useState(false); 
  const [isDarkMode, setIsDarkMode] = useState(true);

  if (isLoading) {
    return null; 
  }

  if (!isAuthenticated) {
    return null; 
  }

  const userTitle = ".NET Developer";

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
