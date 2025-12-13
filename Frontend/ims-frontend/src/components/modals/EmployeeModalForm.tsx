import React, { useState, useEffect } from 'react';
import type { GetEmployeeByIdQueryResponse, RegisterEmployeeCommand, ChangeRoleCommand, MoveToDepartmentCommand } from '../../entities/recruitment/dto/employee_dto';
import type { GetDepartmentByIdQueryResponse } from '../../entities/recruitment/dto/department_dto';
import { EmployeeRole } from '../../entities/recruitment/enums';
import styles from '../common/commonStyles/commonModalStyles.module.css'
import {departmentService, employeeService} from "../../api/services/recruitment";
import ModalField from "../ModalField.tsx";
import ModalSelect from "../ModalSelect.tsx";
import ModalWrapper from "../ModalWrapper.tsx";

interface EmployeeFormModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSuccess: () => void;
    initialEmployee?: GetEmployeeByIdQueryResponse;
}

const getInitialFormData = (employee?: GetEmployeeByIdQueryResponse): RegisterEmployeeCommand => ({
    firstName: employee?.firstName || '',
    lastName: employee?.lastName || '',
    email: employee?.email || '',
    role: employee?.role || EmployeeRole.Developer,
    departmentId: employee?.departmentId || '',
    patronymic: employee?.patronymic || '',
});

const EmployeeFormModal: React.FC<EmployeeFormModalProps> = ({ isOpen, onClose, onSuccess, initialEmployee }) => {
    const isEditMode = !!initialEmployee;
    const [formData, setFormData] = useState<RegisterEmployeeCommand>(getInitialFormData(initialEmployee));
    const [departments, setDepartments] = useState<GetDepartmentByIdQueryResponse[]>([]);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const modalTitle = isEditMode ? 'Edit Employee' : 'Register New Employee';

    useEffect(() => {
        if (!isOpen) return;

        setFormData(getInitialFormData(initialEmployee));
        setError(null);

        const fetchDepartments = async () => {
            try {
                const response = await departmentService.getAllDepartments(1, 100);
                const items = response.departments.items || [];
                setDepartments(items);

                if (!initialEmployee?.departmentId && items.length > 0) {
                    setFormData(prev => ({ ...prev, departmentId: items[0].id }));
                }
            } catch (e) {
                console.error("Failed to fetch departments:", e);
                setError("Could not load department list.");
            }
        };
        fetchDepartments();

    }, [isOpen, initialEmployee]);


    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;

        setFormData(prev => ({
            ...prev,
            [name]: name === 'role' ? Number(value) : value,
        }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!formData.departmentId) return setError("Department is required.");

        setIsSubmitting(true);
        setError(null);

        try {
            if (isEditMode && initialEmployee) {
                const changeRoleCommand: ChangeRoleCommand = {
                    employeeId: initialEmployee.id,
                    newRole: formData.role,
                };
                const moveToDeptCommand: MoveToDepartmentCommand = {
                    employeeId: initialEmployee.id,
                    departmentId: formData.departmentId,
                };

                if (formData.role !== initialEmployee.role) {
                    await employeeService.changeRole(changeRoleCommand);
                }
                if (formData.departmentId !== initialEmployee.id) {
                    await employeeService.moveToDepartment(moveToDeptCommand);
                }
            } else {
                await employeeService.registerEmployee(formData);
            }

            onSuccess();
            onClose();
        } catch (err: any) {
            console.error("Employee submission failed:", err);
            setError(err.response?.data?.message || `Failed to ${isEditMode ? 'update' : 'register'} employee.`);
        } finally {
            setIsSubmitting(false);
        }
    };

    if (!isOpen) return null;

    const roleOptions = Object.keys(EmployeeRole).filter(key => Number.isNaN(Number(key))).map(key => ({
        value: String(EmployeeRole[key as keyof typeof EmployeeRole]),
        label: key.replaceAll(/([A-Z])/g, ' $1').trim()
    }));

    return (
        <ModalWrapper isOpen={isOpen} onClose={onClose} title={modalTitle}  error={error} >
            <form onSubmit={handleSubmit} className={styles.form}>
                <ModalField label="First Name" name="firstName" value={formData.firstName || ''} onChange={handleChange} required disabled={isEditMode} />
                <ModalField label="Last Name" name="lastName" value={formData.lastName || ''} onChange={handleChange} required disabled={isEditMode} />
                <ModalField label="Email" name="email" type="email" value={formData.email || ''} onChange={handleChange} required disabled={isEditMode} />
                <ModalSelect label="Role" name="role" value={String(formData.role)} onChange={handleChange} required options={roleOptions} />
                <ModalSelect label="Department" name="departmentId" value={formData.departmentId} onChange={handleChange} required
                             options={departments.map(d => ({ value: d.id, label: d.name || 'N/A' }))}
                />
                <button type="submit" disabled={isSubmitting} className={styles.submitButton}>
                    {isSubmitting ? 'Saving...' : (isEditMode ? 'Save Changes' : 'Register Employee')}
                </button>
            </form>
        </ModalWrapper>
    );
};

export default EmployeeFormModal;
