import React from 'react';
import styles from './LoginPage.module.css';

interface LoginPageProps {
  onLogin: () => void;
}

const LoginPage: React.FC<LoginPageProps> = ({ onLogin }) => {
  return (
    <div className={styles.loginPage}>
      <div className={styles.loginContainer}>
        <div className={styles.logoPlaceholder}>
          {/* Placeholder for a logo */}
        </div>
        <h1 className={styles.headline}>Welcome to IMS</h1>
        <form className={styles.loginForm}>
          <button type="button" onClick={onLogin} className={styles.loginButton}>
            Log In
          </button>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;
