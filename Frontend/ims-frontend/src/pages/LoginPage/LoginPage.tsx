import React from 'react';
import styles from './LoginPage.module.css';

interface LoginPageProps {
  onLogin: () => void;
}

const LoginPage: React.FC<LoginPageProps> = ({ onLogin }) => {
  return (
    <div className={styles.loginPage}>
      <div className={styles.blob} />
      
      <div className={styles.loginContainer}>
        <div className={styles.brandHeader}>
          <div className={styles.logoIcon}>
            <svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M20 7H4C2.89543 7 2 7.89543 2 9V19C2 20.1046 2.89543 21 4 21H20C21.1046 21 22 20.1046 22 19V9C22 7.89543 21.1046 7 20 7Z"
                    stroke="currentColor"
                    strokeWidth="2" 
                    strokeLinecap="round" 
                    strokeLinejoin="round"/>
                    
              <path d="M16 21V5C16 4.46957 15.7893 3.96086 15.4142 3.58579C15.0391 3.21071 14.5304 3 14 3H10C9.46957 3 8.96086 3.21071 8.58579 3.58579C8.21071 3.96086 8 4.46957 8 5V21" 
                    stroke="currentColor" 
                    strokeWidth="2" 
                    strokeLinecap="round" 
                    strokeLinejoin="round"/>
            </svg>
          </div>
          <h1 className={styles.headline}>IMS Portal</h1>
          <p className={styles.subtitle}>Streamlining recruitment and intern success.</p>
        </div>

        <form className={styles.loginForm}>
          <button type="button" onClick={onLogin} className={styles.loginButton}>
            Sign in to your account
          </button>
        </form>
        
        <p className={styles.footerText}>
          &copy; {new Date().getFullYear()} Intern Management System
        </p>
      </div>
    </div>
  );
};

export default LoginPage;
