import React, { useState, useEffect } from 'react';
import type { GetInterviewByIdQueryResponse, RescheduleInterviewCommand } from '../../entities/recruitment/dto/interview_dto';
import styles from '../common/commonStyles/commonModalStyles.module.css'
import {interviewService} from "../../api/services/recruitment";

interface RescheduleInterviewModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSuccess: () => void;
    interview?: GetInterviewByIdQueryResponse;
}

const formatISOToLocal = (isoString: string) => {
    if (!isoString) return '';
    const date = new Date(isoString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');

    return `${year}-${month}-${day}T${hours}:${minutes}`;
};

const convertLocalToUTC = (localDateTime: string): string => {
    return new Date(localDateTime).toISOString();
};

const RescheduleInterviewModal: React.FC<RescheduleInterviewModalProps> = ({ isOpen, onClose, onSuccess, interview }) => {
    const [newDate, setNewDate] = useState(formatISOToLocal(interview?.scheduledAt || ''));
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (isOpen && interview) {
            setNewDate(formatISOToLocal(interview.scheduledAt));
            setError(null);
        }
    }, [isOpen, interview]);


    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!interview || !interview.id) return;

        setIsSubmitting(true);
        setError(null);

        try {
            const command: RescheduleInterviewCommand = {
                id: interview.id,
                newDate: convertLocalToUTC(newDate),
            };

            await interviewService.rescheduleInterview(command);
            onSuccess();
            onClose();
        } catch (err: any) {
            setError(err.response?.data?.message || 'Failed to reschedule interview.');
        } finally {
            setIsSubmitting(false);
        }
    };

    if (!isOpen || !interview) return null;

    return (
        <div className={styles.modalOverlay}>
            <div className={styles.modalContent}>
                <h2 className={styles.modalTitle}>Reschedule Interview for {interview.candidateEmail}</h2>
                <button className={styles.closeButton} onClick={onClose}>&times;</button>

                <form onSubmit={handleSubmit} className={styles.form}>
                    {error && <div className={styles.error}>{error}</div>}

                    <label>Current Date/Time</label>
                    <input type="text" value={formatISOToLocal(interview.scheduledAt).replace('T', ' ')} disabled />

                    <label>New Scheduled Date/Time</label>
                    <input
                        type="datetime-local"
                        value={newDate}
                        onChange={(e) => setNewDate(e.target.value)}
                        required
                    />

                    <button type="submit" disabled={isSubmitting} className={styles.submitButton}>
                        {isSubmitting ? 'Rescheduling...' : 'Confirm Reschedule'}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default RescheduleInterviewModal;
