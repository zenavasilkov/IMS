import React, { useState, useEffect } from 'react';
import type { GetDepartmentByIdQueryResponse, AddDepartmentCommand } from '../../entities/recruitment/dto/department_dto';
import styles from '../common/commonStyles/commonModalStyles.module.css'
import {departmentService} from "../../api/services/recruitment";

interface DepartmentFormModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSuccess: () => void;
    initialDepartment?: GetDepartmentByIdQueryResponse; // If present, Edit Mode
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
        <div className={styles.modalOverlay} onClick={onClose}>
            <div className={styles.modalContent} onClick={e => e.stopPropagation()}>
                <h2 className={styles.modalTitle}>{isEditMode ? 'Edit Department' : 'Add New Department'}</h2>
                <button className={styles.closeButton} onClick={onClose}>&times;</button>

                <form onSubmit={handleSubmit} className={styles.form}>
                    {error && <div className={styles.error}>{error}</div>}

                    <label>Department Name<input name="name" value={formData.name} onChange={handleChange} required /></label>

                    <label>
                        Description (Optional)<br/>
                        <textarea
                            name="description"
                            value={formData.description}
                            onChange={handleChange as any}
                            rows={2}
                        />
                    </label>

                    <button type="submit" disabled={isSubmitting} className={styles.submitButton}>
                        {isSubmitting ? 'Saving...' : (isEditMode ? 'Save Changes' : 'Add Department')}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default DepartmentFormModal;
