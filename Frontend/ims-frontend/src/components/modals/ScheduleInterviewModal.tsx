import React, { useState, useEffect } from 'react';
import type { FindCandidateByIdQueryResponse } from '../../entities/recruitment/dto/candidate_dto.ts';
import type { ScheduleInterviewCommand } from '../../entities/recruitment/dto/interview_dto.ts';
import type { GetDepartmentByIdQueryResponse } from '../../entities/recruitment/dto/department_dto.ts';
import type { GetEmployeeByIdQueryResponse } from '../../entities/recruitment/dto/employee_dto.ts';
import {EmployeeRole, InterviewType} from '../../entities/recruitment/enums.ts';
import styles from '../common/commonStyles/commonModalStyles.module.css'
import {departmentService, employeeService, interviewService} from "../../api/services/recruitment";
import {convertLocalToUTC} from "../../features/helpers/TimeConverter.ts";

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

        const updatedValue = name === 'type' ? parseInt(value) as InterviewType : value;

        setFormData(prev => ({
            ...prev,
            [name]: updatedValue,
        }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!candidate || !candidate.id) return;

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

    const interviewTypeOptions = Object.keys(InterviewType).filter(key => isNaN(Number(key))).map(key => ({
        value: String(InterviewType[key as keyof typeof InterviewType]),
        label: key.replace(/([A-Z])/g, ' $1').trim()
    }));

    const getEmployeeRoleDisplay = (role: EmployeeRole) => {
        return EmployeeRole[role].replace(/([A-Z])/g, ' $1').trim();
    };

    return (
        <div className={styles.modalOverlay}>
            <div className={styles.modalContent}>
                <h2 className={styles.modalTitle}>Schedule Interview for {candidate.firstName} {candidate.lastName}</h2>
                <button className={styles.closeButton} onClick={onClose}>&times;</button>

                <form onSubmit={handleSubmit} className={styles.form}>
                    {error && <div className={styles.error}>{error}</div>}

                    <label>
                        Department<select name="departmentId" value={formData.departmentId} onChange={handleChange} required>
                        {departments.map(d => (
                            <option key={d.id} value={d.id}>{d.name}</option>
                        ))}
                    </select></label>

                    <label>
                        Interviewer<select name="interviewerId" value={formData.interviewerId} onChange={handleChange} required>
                        {interviewers.map(e => (
                            <option key={e.id} value={e.id}>
                                {e.firstName} {e.lastName} ({getEmployeeRoleDisplay(e.role)})
                            </option>
                        ))}
                    </select></label>

                    <label>
                        Interview Type<select name="type" value={String(formData.type)} onChange={handleChange} required>
                        {interviewTypeOptions.map(opt => (
                            <option key={opt.value} value={opt.value}>{opt.label}</option>
                        ))}
                    </select></label>

                    <label>
                        Scheduled At<input
                        name="scheduledAt"
                        type="datetime-local"
                        value={formData.scheduledAt}
                        onChange={handleChange}
                        required
                    /></label>

                    <button type="submit" disabled={isSubmitting} className={styles.submitButton}>
                        {isSubmitting ? 'Scheduling...' : 'Schedule Interview'}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default ScheduleInterviewModal;
