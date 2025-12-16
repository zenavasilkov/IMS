import React, { useState, useEffect } from 'react';
import styles from '../common/commonStyles/commonModalStyles.module.css'
import ModalWrapper from './modalComponents/ModalWrapper.tsx';
import ModalField from './modalComponents/ModalField.tsx';
import ModalSelect from './modalComponents/ModalSelect.tsx';
import {internshipService} from "../../api/services";
import {InternshipStatus, Role} from "../../entities/ims/enums.ts";
import type {
    CreateInternshipDto,
    InternshipDto,
    UpdateInternshipDto
} from "../../entities/ims/dto/internship_dto.ts";
import type {UserDto} from "../../entities/ims/dto/user_dto.ts";
import UserSearchSelect from "../UserSearchSelect.tsx";
import {convertLocalToUTC} from "../../features/helpers/TimeConverter.ts";

interface InternshipFormModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSuccess: () => void;
    initialInternship?: InternshipDto;
    currentUserDto?: UserDto;
}

const getInitialFormData = (internship?: InternshipDto): CreateInternshipDto => ({
    internId: internship?.internId || '',
    mentorId: internship?.mentorId || '',
    humanResourcesManagerId: internship?.humanResourcesManagerId || '',
    startDate: internship?.startDate.slice(0, 10) || new Date().toISOString().slice(0, 10),
    endDate: internship?.endDate?.slice(0, 10) || '',
    status: internship?.status || InternshipStatus.NotStarted,
});

const InternshipFormModal: React.FC<InternshipFormModalProps> = ({ isOpen, onClose, onSuccess, initialInternship, currentUserDto }) => {

    const isEditMode = !!initialInternship;
    const [formData, setFormData] = useState<CreateInternshipDto | UpdateInternshipDto>(getInitialFormData(initialInternship));
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const submitButtonContent = isEditMode ? 'Save Changes' : 'Create Internship'

    useEffect(() => {
        setFormData(getInitialFormData(initialInternship));
        setError(null);

        if (currentUserDto) {
            setFormData(prev => ({
                ...prev,
                humanResourcesManagerId: currentUserDto.id
            }));
        }
    }, [isOpen, initialInternship, currentUserDto]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        setFormData(prev => ({ ...prev, [e.target.name]: e.target.name === 'status' ? Number(e.target.value) : e.target.value }));
    };

    const handleUserSelect = (name: 'internId' | 'mentorId') => (id: string) => {
        setFormData(prev => {
            return { ...prev, [name]: id } as CreateInternshipDto | UpdateInternshipDto;
        });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!(formData as CreateInternshipDto).internId || !formData.mentorId) {
            setError("Intern and Mentor must be selected.");
            return;
        }

        const utcStartDate = convertLocalToUTC(formData.startDate);
        const utcEndDate = formData.endDate ? convertLocalToUTC(formData.endDate) : null;

        if (!utcStartDate) {
            setError("Start Date is required and must be valid.");
            setIsSubmitting(false);
            return;
        }

        setIsSubmitting(true);
        setError(null);

        try {
            if (isEditMode && initialInternship) {
                const updateData: UpdateInternshipDto = {
                    ...(formData as UpdateInternshipDto),
                    startDate: utcStartDate,
                    endDate: utcEndDate,
                };
                await internshipService.updateInternship(initialInternship.id, updateData);
            } else {
                const createData: CreateInternshipDto = {
                    ...(formData as CreateInternshipDto),
                    startDate: utcStartDate,
                    endDate: utcEndDate,
                    status: formData.status!,
                };
                await internshipService.createInternship(createData);
            }
            onSuccess();
            onClose();
        } catch (err: any) {
            setError(err.response?.data?.message || `Failed to ${isEditMode ? 'update' : 'create'} internship.`);
        } finally {
            setIsSubmitting(false);
        }
    };

    if (!isOpen) return null;

    const statusOptions = Object.keys(InternshipStatus).filter(key => Number.isNaN(Number(key))).map(key => ({
        value: String(InternshipStatus[key as keyof typeof InternshipStatus]),
        label: key.replaceAll(/([A-Z])/g, ' $1').trim()
    }));


    return (
        <ModalWrapper isOpen={isOpen} onClose={onClose} title={isEditMode ? 'Edit Internship' : 'Create New Internship'} error={error}>
            <form onSubmit={handleSubmit} className={styles.form}>

                <ModalField
                    label="HR Manager"
                    name="hrManager"
                    value={currentUserDto ? `${currentUserDto.firstName || ''} ${currentUserDto.lastName || ''} (${currentUserDto.email || 'N/A'})` : 'Loading...'}
                    disabled
                />

                <UserSearchSelect
                    label="Intern"
                    currentUserId={(formData as CreateInternshipDto).internId || ''}
                    onSelect={handleUserSelect('internId')}
                    required
                    disabled={isEditMode}
                    filterRole={Role.Intern}
                    displayFullName={true}
                />

                <UserSearchSelect
                    label="Mentor"
                    currentUserId={(formData as CreateInternshipDto).mentorId || ''}
                    onSelect={handleUserSelect('mentorId')}
                    required
                    filterRole={Role.Mentor}
                    displayFullName={true}
                />

                <ModalField label="Start Date" name="startDate" type="date" value={formData.startDate} onChange={handleChange} required />

                <ModalField label="End Date (Optional)" name="endDate" type="date" value={formData.endDate || ''} onChange={handleChange} />

                <ModalSelect label="Status" name="status" value={String(formData.status)} onChange={handleChange} required options={statusOptions} />

                <button type="submit" disabled={isSubmitting} className={styles.submitButton}>
                    {isSubmitting ? 'Saving...' : submitButtonContent}
                </button>
            </form>
        </ModalWrapper>
    );
};

export default InternshipFormModal;
