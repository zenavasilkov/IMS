import React from 'react';
import styles from './common/commonStyles/commonPageStyles.module.css';

interface PaginationControlsProps {
  currentPage: number;
  totalPages: number;
  onPreviousPage: () => void;
  onNextPage: () => void;
  hasContent: boolean;
}

const PaginationControls: React.FC<PaginationControlsProps> = ({
  currentPage,
  totalPages,
  onPreviousPage,
  onNextPage,
  hasContent,
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
        disabled={currentPage === totalPages || !hasContent}
        className={styles.button}
      >
        Next
      </button>
    </div>
  );
};

export default PaginationControls;
