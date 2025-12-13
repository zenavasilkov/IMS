import React, {useEffect, useState} from 'react';
import type { RegisterCandidateCommand } from '../../entities/recruitment/dto/candidate_dto.ts';
import styles from '../common/commonStyles/commonModalStyles.module.css'
import {candidateService} from "../../api/services/recruitment";

interface RegisterCandidateModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSuccess: () => void;
}

const EMPTY_FORM_DATA: RegisterCandidateCommand = {
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    cvLink: '',
    linkedIn: '',
    patronymic: '',
};

const PHONE_SANITIZE_REGEX = /[^0-9+]/g;
const PHONE_NUMBER_REGEX = String.raw`^\+?[1-9]\d{1,14}$`;

const RegisterCandidateModal: React.FC<RegisterCandidateModalProps> = ({ isOpen, onClose, onSuccess }) => {

    const [formData, setFormData] = useState<RegisterCandidateCommand>(EMPTY_FORM_DATA);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (isOpen) {
            setFormData(EMPTY_FORM_DATA);
            setError(null);
        }
    }, [isOpen]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        let newValue = value;

        if (name === 'phoneNumber') {
            newValue = value.replace(PHONE_SANITIZE_REGEX, '');
        }

        setFormData(prev => ({ ...prev, [name]: newValue }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setIsSubmitting(true);
        setError(null);

        try {
            await candidateService.registerCandidate(formData);
            onSuccess();
            onClose();
        } catch (err: any) {
            console.error("Candidate registration failed:", err);
            setError(err.response?.data?.message || 'Failed to register candidate.');
        } finally {
            setIsSubmitting(false);
        }
    };

    if (!isOpen) return null;

    return (
        <div className={styles.modalOverlay}>
            <div className={styles.modalContent}>
                <h2 className={styles.modalTitle}>Register New Candidate</h2>
                <button className={styles.closeButton} onClick={onClose}>&times;</button>

                <form onSubmit={handleSubmit} className={styles.form}>
                    {error && <div className={styles.error}>{error}</div>}

                    <label>First Name<input name="firstName" value={formData.firstName || ''} onChange={handleChange} required /></label>
                    <label>Last Name<input name="lastName" value={formData.lastName || ''} onChange={handleChange} required /></label>
                    <label>Email<input name="email" type="email" value={formData.email || ''} onChange={handleChange} required /></label>
                    <label>Phone Number<input
                        name="phoneNumber"
                        type="tel" value={formData.phoneNumber || ''}
                        onChange={handleChange}
                        pattern={PHONE_NUMBER_REGEX}
                        title="Must be a valid phone number format."
                    /></label>
                    <label>CV Link</label><input name="cvLink" type="url" value={formData.cvLink || ''} onChange={handleChange} />
                    <label>LinkedIn Profile</label><input name="linkedIn" type="url" value={formData.linkedIn || ''} onChange={handleChange} />

                    <button type="submit" disabled={isSubmitting} className={styles.submitButton}>
                        {isSubmitting ? 'Registering...' : 'Register Candidate'}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default RegisterCandidateModal;
