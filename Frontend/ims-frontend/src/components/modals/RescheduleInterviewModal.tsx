import React, { useState, useEffect } from 'react';
import type { GetInterviewByIdQueryResponse, RescheduleInterviewCommand } from '../../entities/recruitment/dto/interview_dto';
import styles from '../common/commonStyles/commonModalStyles.module.css'
import {interviewService} from "../../api/services/recruitment";
import ModalWrapper from "../ModalWrapper.tsx";
import ModalField from "../ModalField.tsx";

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
        if (!interview?.id) return;

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

    const modalTitle = `Reschedule Interview for ${interview.candidateEmail}`;
    const currentDateTimeDisplay = formatISOToLocal(interview.scheduledAt).replaceAll('T', ' ');

    return (
        <ModalWrapper isOpen={isOpen} onClose={onClose} title={modalTitle} error={error}>
            <form onSubmit={handleSubmit} className={styles.form}>
                <ModalField label="Current Date/Time" type="text" value={currentDateTimeDisplay} disabled />
                <ModalField label="New Scheduled Date/Time" type="datetime-local" value={newDate} onChange={(e) => setNewDate(e.target.value)} required/>
                <button type="submit" disabled={isSubmitting} className={styles.submitButton}> {isSubmitting ? 'Rescheduling...' : 'Confirm Reschedule'} </button>
            </form>
        </ModalWrapper>
    );
};

export default RescheduleInterviewModal;
