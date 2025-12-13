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
import PaginationControls from "../../components/PaginationControls.tsx";
import PageLayout from "../../components/PageLayout.tsx";
import SimpleListHeader from "../../components/SimpleListHeader.tsx";
import useMinLoadingTime from "../../hooks/useMinLoadingTime.ts";

const EMPLOYEE_HEADER_CONFIG = [
    { label: 'Name / Contact', flex: 3, textAlign: 'left' as const },
    { label: 'Department', flex: 7, textAlign: 'center' as const },
    { label: 'Actions', flex: 2, textAlign: 'center' as const },
];

const EmployeePage: React.FC = () => {
    const { isAuthenticated, isLoading: isAuth0Loading } = useAuth0();
    const dispatch = useAppDispatch();
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [employeeToEdit, setEmployeeToEdit] = useState<GetEmployeeByIdQueryResponse | undefined>(undefined);
    const { employees, loading, error, page, totalPages, pageSize } = useSelector((state: RootState) => state.employee);
    const [departments, setDepartments] = useState<any[]>([]);
    const showPageLoader = useMinLoadingTime(loading || isAuth0Loading);

    const getDepartmentName = (id: string) => departments.find(d => d.id === id)?.name || 'N/A';

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
        setEmployeeToEdit(employee);
        setIsModalOpen(true);
    };

    if (showPageLoader) return <PageLoader loadingText="Loading employees..." />;
    if (error) return <div className={commonStyles.errorMessage}>{error}</div>;

    return(
        <PageLayout
            title="Employee Management"
            Icon={EmployeeIcon}
            iconColor="#28a745"
            createButton={<button className={commonStyles.createButton} onClick={handleAddEmployee}>Add New Employee</button>}
        >
            <div className={styles.employeeList}>
                <SimpleListHeader columns={EMPLOYEE_HEADER_CONFIG} />
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

            <PaginationControls
                currentPage={page}
                totalPages={totalPages}
                onPreviousPage={() => dispatch(setEmployeePage(page - 1))}
                onNextPage={() => dispatch(setEmployeePage(page + 1))}
                hasContent={departments.length > 0}
            />

            <EmployeeFormModal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                onSuccess={handleSuccess}
                initialEmployee={employeeToEdit}
            />
        </PageLayout>
    );
};

export default EmployeePage;
