import React, { useState, useEffect } from 'react';
import type { FindCandidateByIdQueryResponse } from '../../entities/recruitment/dto/candidate_dto.ts';
import type { ScheduleInterviewCommand } from '../../entities/recruitment/dto/interview_dto.ts';
import type { GetDepartmentByIdQueryResponse } from '../../entities/recruitment/dto/department_dto.ts';
import type { GetEmployeeByIdQueryResponse } from '../../entities/recruitment/dto/employee_dto.ts';
import {EmployeeRole, InterviewType} from '../../entities/recruitment/enums.ts';
import styles from '../common/commonStyles/commonModalStyles.module.css'
import {departmentService, employeeService, interviewService} from "../../api/services/recruitment";
import {convertLocalToUTC} from "../../features/helpers/TimeConverter.ts";
import ModalWrapper from "./modalComponents/ModalWrapper.tsx";
import ModalSelect from "./modalComponents/ModalSelect.tsx";
import ModalField from "./modalComponents/ModalField.tsx";

interface ScheduleInterviewModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSuccess: () => void;
    candidate?: FindCandidateByIdQueryResponse;
}

const getInitialDateTime = () => new Date(Date.now() + 60 * 60 * 1000).toISOString().slice(0, 16);

const ScheduleInterviewModal: React.FC<ScheduleInterviewModalProps> = ({ isOpen, onClose, onSuccess, candidate }) => {
    const [departments, setDepartments] = useState<GetDepartmentByIdQueryResponse[]>([]);
    const [interviewers, setInterviewers] = useState<GetEmployeeByIdQueryResponse[]>([]);
    const [isOptionsLoading, setIsOptionsLoading] = useState(false);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const departmentOptions = departments.map(d =>
        ({ value: d.id, label: d.name || 'N/A' }));

    const getEmployeeRoleDisplay = (role: EmployeeRole) =>
        EmployeeRole[role].replaceAll(/([A-Z])/g, ' $1').trim();

    const interviewerOptions = interviewers.map(e =>
        ({ value: e.id, label: `${e.firstName} ${e.lastName} (${getEmployeeRoleDisplay(e.role)})` }));

    const [formData, setFormData] = useState<Omit<ScheduleInterviewCommand, 'candidateId'>>({
        interviewerId: '',
        departmentId: '',
        type: InterviewType.Screening,
        scheduledAt: getInitialDateTime(),
    });

    useEffect(() => {
        if (!isOpen) return;

        const fetchOptions = async () => {
            setIsOptionsLoading(true);
            setError(null);
            try {
                const depResponse = await departmentService.getAllDepartments(1, 100);
                const deptItems = depResponse.departments.items || [];
                setDepartments(deptItems);

                const empResponse = await employeeService.getAllEmployees(1, 100);
                const empItems = empResponse.employees.items || [];
                setInterviewers(empItems);

                setFormData(prev => ({
                    ...prev,
                    departmentId: deptItems[0]?.id || '',
                    interviewerId: empItems[0]?.id || '',
                }));
            } catch (e) {
                console.error("Failed to fetch modal options:", e);
                setError("Could not load interview options (departments/interviewers).");
            } finally {
                setIsOptionsLoading(false);
            }
        };
        fetchOptions();
    }, [isOpen]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;

        const updatedValue = name === 'type' ? Number.parseInt(value) as InterviewType : value;

        setFormData(prev => ({
            ...prev,
            [name]: updatedValue,
        }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!candidate?.id) return;

        setIsSubmitting(true);
        setError(null);

        try {
            const command: ScheduleInterviewCommand = {
                ...formData,
                candidateId: candidate.id,
                scheduledAt: convertLocalToUTC(formData.scheduledAt),
            };

            await interviewService.scheduleInterview(command);

            onSuccess();
            onClose();
        } catch (err: any) {
            console.error("Interview scheduling failed:", err);
            setError(err.response?.data?.message || 'Failed to schedule interview.');
        } finally {
            setIsSubmitting(false);
        }
    };

    if (!isOpen || !candidate) return null;
    if (isOptionsLoading) return <div className={styles.modalOverlay}><div className={styles.loadingMessage}>Loading options...</div></div>;

    const interviewTypeOptions = Object.keys(InterviewType).filter(key => Number.isNaN(Number(key))).map(key => ({
        value: String(InterviewType[key as keyof typeof InterviewType]),
        label: key.replaceAll(/([A-Z])/g, ' $1').trim()
    }));

    return (
        <ModalWrapper isOpen={isOpen} onClose={onClose} title={`Schedule Interview for ${candidate?.firstName} ${candidate?.lastName}`} error={error} >
            <form onSubmit={handleSubmit} className={styles.form}>
                <ModalSelect label="Department" name="departmentId" value={formData.departmentId} onChange={handleChange} required options={departmentOptions} />
                <ModalSelect label="Interviewer" name="interviewerId" value={formData.interviewerId} onChange={handleChange} required options={interviewerOptions} />
                <ModalSelect label="Interview Type" name="type" value={String(formData.type)} onChange={handleChange} required options={interviewTypeOptions} />
                <ModalField label="Scheduled At" name="scheduledAt" type="datetime-local" value={formData.scheduledAt} onChange={handleChange} required />

                <button type="submit" disabled={isSubmitting} className={styles.submitButton}> {isSubmitting ? 'Scheduling...' : 'Schedule Interview'} </button>
            </form>
        </ModalWrapper>
    );
};

export default ScheduleInterviewModal;
