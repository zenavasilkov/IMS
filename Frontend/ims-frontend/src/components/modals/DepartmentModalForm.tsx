import React, { useState, useEffect } from 'react';
import type { GetDepartmentByIdQueryResponse, AddDepartmentCommand } from '../../entities/recruitment/dto/department_dto';
import styles from '../common/commonStyles/commonModalStyles.module.css'
import {departmentService} from "../../api/services/recruitment";
import ModalWrapper from "../ModalWrapper.tsx";
import ModalField from "../ModalField.tsx";

interface DepartmentFormModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSuccess: () => void;
    initialDepartment?: GetDepartmentByIdQueryResponse;
}

const getInitialFormData = (dept?: GetDepartmentByIdQueryResponse) => ({
    name: dept?.name || '',
    description: dept?.description || '',
});

const DepartmentFormModal: React.FC<DepartmentFormModalProps> = ({ isOpen, onClose, onSuccess, initialDepartment }) => {
    const isEditMode = !!initialDepartment;
    const [formData, setFormData] = useState(getInitialFormData(initialDepartment));
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    let buttonText = 'Add Department';
    if (isEditMode) { buttonText = 'Save Changes'; }
    if (isSubmitting) { buttonText = 'Saving...'; }

    useEffect(() => {
        if (isOpen) {
            setFormData(getInitialFormData(initialDepartment));
            setError(null);
        }
    }, [isOpen, initialDepartment]);


    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setIsSubmitting(true);
        setError(null);

        try {
            if (isEditMode && initialDepartment) {
                await departmentService.renameDepartment({
                    id: initialDepartment.id,
                    newName: formData.name,
                });
                await departmentService.updateDescription({
                    id: initialDepartment.id,
                    newDescription: formData.description,
                });
            } else {
                const command: AddDepartmentCommand = {
                    name: formData.name,
                    description: formData.description,
                };
                await departmentService.addDepartment(command);
            }

            onSuccess();
            onClose();
        } catch (err: any) {
            console.error("Department submission failed:", err);
            setError(err.response?.data?.message || `Failed to ${isEditMode ? 'update' : 'add'} department.`);
        } finally {
            setIsSubmitting(false);
        }
    };

    if (!isOpen) return null;

    return (
        <ModalWrapper isOpen={isOpen} onClose={onClose} title={isEditMode ? 'Edit Department' : 'Add New Department'} error={error}>
            <form onSubmit={handleSubmit} className={styles.form}>
                <ModalField label="Department Name" name="name" value={formData.name} onChange={handleChange} required />

                <label>
                    Description (Optional) <br/>
                    <textarea name="description" value={formData.description} onChange={handleChange as any} rows={3} className={styles.formInput} />
                </label>

                <button type="submit" disabled={isSubmitting} className={styles.submitButton}> {buttonText} </button>
            </form>
        </ModalWrapper>
    );
};

export default DepartmentFormModal;
