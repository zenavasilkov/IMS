import React, { useState, useEffect } from 'react';
import type { UpdateCvLinkCommand } from '../../entities/recruitment/dto/candidate_dto';
import styles from '../common/commonStyles/commonModalStyles.module.css'
import {useAppDispatch} from "../useAppDispatch.ts";
import {updateCandidateCv} from "../../features/slices/recruitmentSlice.ts"; // Create simple CSS

interface UpdateCvModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSuccess: () => void;
    candidateId: string;
    currentCvLink: string | null | undefined;
}

const UpdateCvModal: React.FC<UpdateCvModalProps> = ({ isOpen, onClose, onSuccess, candidateId, currentCvLink }) => {
    const dispatch = useAppDispatch();
    const [newCvLink, setNewCvLink] = useState(currentCvLink || '');
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (isOpen) {
            setNewCvLink(currentCvLink || '');
            setError(null);
        }
    }, [isOpen, currentCvLink]);


    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setIsSubmitting(true);
        setError(null);

        const command: UpdateCvLinkCommand = {
            id: candidateId,
            newCvLink: newCvLink
        };

        try {
            await dispatch(updateCandidateCv(command)).unwrap();
            onSuccess();
        } catch (err: any) {
            setError(err.message || 'Failed to update CV link.');
        } finally {
            setIsSubmitting(false);
        }
    };

    if (!isOpen) return null;

    return (
        <div className={styles.modalOverlay}>
            <div className={styles.modalContent}>
                <h2 className={styles.modalTitle}>Update CV Link</h2>
                <button className={styles.closeButton} onClick={onClose}>&times;</button>

                <form onSubmit={handleSubmit} className={styles.form}>
                    {error && <div className={styles.error}>{error}</div>}

                    <label>New CV Link (URL)</label>
                    <input
                        type="url"
                        value={newCvLink}
                        onChange={(e) => setNewCvLink(e.target.value)}
                        required
                    />

                    <button type="submit" disabled={isSubmitting} className={styles.submitButton}>
                        {isSubmitting ? 'Updating...' : 'Save Link'}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default UpdateCvModal;
