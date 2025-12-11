import React from 'react';
import styles from './common/commonStyles/commonPageStyles.module.css'

interface CreateUserButtonProps {
  onClick: () => void;
}

const CreateUserButton: React.FC<CreateUserButtonProps> = ({ onClick }) => {
  return (
    <button 
      onClick={onClick} 
      className={styles.createButton}
    >
      Create New User
    </button>
  );
};

export default CreateUserButton;
