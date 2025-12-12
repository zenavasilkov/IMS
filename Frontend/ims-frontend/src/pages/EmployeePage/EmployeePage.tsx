import React, {useCallback, useEffect, useState} from 'react';
import { useSelector } from 'react-redux';
import { useAuth0 } from '@auth0/auth0-react';
import type { RootState } from '../../store';
import { useAppDispatch } from '../../components/useAppDispatch';
import PageLoader from '../../components/common/pageLoader/PageLoader';
import styles from './EmployeePage.module.css';
import {fetchEmployees, setEmployeePage} from "../../features/slices/employeeSlice.ts";
import { EmployeeIcon } from "../../components/common/Icons.tsx";
import commonStyles from '../../components/common/commonStyles/commonPageStyles.module.css'
import type {GetEmployeeByIdQueryResponse} from "../../entities/recruitment/dto/employee_dto.ts";
import EmployeeFormModal from "../../components/modals/EmployeeModalForm.tsx";
import {EmployeeRole} from "../../entities/recruitment/enums.ts";
import {departmentService} from "../../api/services/recruitment";

const EmployeePage: React.FC = () => {
    const { isAuthenticated, isLoading: isAuth0Loading } = useAuth0();
    const dispatch = useAppDispatch();
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [employeeToEdit, setEmployeeToEdit] = useState<GetEmployeeByIdQueryResponse | undefined>(undefined);
    const { employees, loading, error, page, totalPages, pageSize } = useSelector((state: RootState) => state.employee);
    const [departments, setDepartments] = useState<any[]>([]);

    const getDepartmentName = (id: string) => {
        return departments.find(d => d.id === id)?.name || 'N/A';
    };

    const getRoleDisplayName = (role: number) => {
        const roleKeyName = EmployeeRole[role];
        return roleKeyName.replaceAll(/([A-Z])/g, ' $1').trim();
    };

    const fetchEmployeesAndDeps = useCallback(async () => {
        if (!isAuth0Loading && isAuthenticated) {
            const depResponse = await departmentService.getAllDepartments(1, 100);
            setDepartments(depResponse.departments.items || []);

            const params = { pageNumber: page, pageSize: pageSize };
            dispatch(fetchEmployees(params));
        }
    }, [page, isAuthenticated, isAuth0Loading, dispatch, pageSize]);

    useEffect(() => {
        fetchEmployeesAndDeps();
    }, [fetchEmployeesAndDeps]);

    const handleSuccess = useCallback(() => {
        setIsModalOpen(false);
        setEmployeeToEdit(undefined);
        fetchEmployeesAndDeps();
    }, [fetchEmployeesAndDeps]);

    const handleAddEmployee = () => {
        setEmployeeToEdit(undefined);
        setIsModalOpen(true);
    };

    const handleEditEmployee = (employee: GetEmployeeByIdQueryResponse) => {
        setEmployeeToEdit(employee); // Set data for EDIT mode
        setIsModalOpen(true);
    };

    if (loading) return <PageLoader loadingText="Loading employees..." />;
    if (error) return <div className={commonStyles.errorMessage}>{error}</div>;

    return (
        <div className={commonStyles.container}>
            <div className={commonStyles.titleArea}>
                <h1 className={commonStyles.heading}>
                    <EmployeeIcon className={styles.headingIcon} />
                    Employee Management
                </h1>

                <button className={commonStyles.createButton} onClick={handleAddEmployee}>Add New Employee</button>
            </div>

            <div className={styles.employeeList}>
                <div className={commonStyles.listHeader}>
                    <span style={{ flex: 3, textAlign: 'left' }}>Name / Role</span>
                    <span style={{ flex: 7 }}>Department</span>
                    <span style={{ flex: 2 }}>Actions</span>
                </div>
                {employees.map(employee => (
                    <div key={employee.id} className={styles.employeeItem}>
                        <div className={styles.employeeInfoColumn} style={{ flex: 3 }}>
                            <span className={styles.name}>
                                {employee.firstName} {employee.lastName}
                                <span className={styles.userRoleText}>({getRoleDisplayName(employee.role)})</span>
                            </span>
                            <span className={styles.subText}>{employee.email}</span>
                        </div>
                        <span className={styles.departmentId}>{getDepartmentName(employee.departmentId)}</span>
                        <div className={styles.actions}>
                            <button className={commonStyles.actionButton} onClick={() => handleEditEmployee(employee)}>Edit</button>
                        </div>
                    </div>
                ))}
            </div>

            <div className={commonStyles.pagination}>
                <button disabled={page === 1} onClick={() => dispatch(setEmployeePage(page - 1))}>Previous</button>
                <span>Page {page} of {totalPages}</span>
                <button disabled={page >= totalPages} onClick={() => dispatch(setEmployeePage(page + 1))}>Next</button>
            </div>

            <EmployeeFormModal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                onSuccess={handleSuccess}
                initialEmployee={employeeToEdit}
            />
        </div>
    );
};

export default EmployeePage;
