import React from 'react';
import styles from '../pages/UserManagementPage/UserManagementPage.module.css';

interface CreateUserButtonProps {
  onClick: () => void;
}

const CreateUserButton: React.FC<CreateUserButtonProps> = ({ onClick }) => {
  return (
    <button 
      onClick={onClick} 
      className={styles.button}
    >
      Create New User
    </button>
  );
};

export default CreateUserButton;
