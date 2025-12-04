import React from 'react';
import styles from '../pages/UserManagementPage/UserManagementPage.module.css';

interface PaginationControlsProps {
  currentPage: number;
  totalPages: number;
  onPreviousPage: () => void;
  onNextPage: () => void;
  hasUsers: boolean;
}

const PaginationControls: React.FC<PaginationControlsProps> = ({
  currentPage,
  totalPages,
  onPreviousPage,
  onNextPage,
  hasUsers,
}) => {
  return (
    <div className={styles.pagination}>
      <button 
        onClick={onPreviousPage} 
        disabled={currentPage === 1}
        className={styles.button}
      >
        Previous
      </button>
      <span className={styles.pageInfo}>Page {currentPage} of {totalPages}</span>
      <button 
        onClick={onNextPage} 
        disabled={currentPage === totalPages || !hasUsers}
        className={styles.button}
      >
        Next
      </button>
    </div>
  );
};

export default PaginationControls;
