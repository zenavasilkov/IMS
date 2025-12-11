import React, {useCallback, useEffect, useState} from 'react';
import { useSelector } from 'react-redux';
import { useAuth0 } from '@auth0/auth0-react';
import type { RootState } from '../../store';
import { useAppDispatch } from '../../components/useAppDispatch';
import PageLoader from '../../components/common/pageLoader/PageLoader';
import styles from './DepartmentPage.module.css';
import commonStyles from '../../components/common/commonStyles/commonPageStyles.module.css'
import {fetchDepartments, setDepartmentPage} from "../../features/slices/departmentSlice.ts";
import { DepartmentIcon } from "../../components/common/Icons.tsx";
import type {GetDepartmentByIdQueryResponse} from "../../entities/recruitment/dto/department_dto.ts";
import DepartmentFormModal from "../../components/modals/DepartmentModalForm.tsx";

const DepartmentPage: React.FC = () => {
    const { isAuthenticated, isLoading: isAuth0Loading } = useAuth0();
    const dispatch = useAppDispatch();
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [deptToEdit, setDeptToEdit] = useState<GetDepartmentByIdQueryResponse | undefined>(undefined);

    const { departments, loading, error, page, totalPages, pageSize } = useSelector((state: RootState) => state.department);

    const fetchDepartmentsData = useCallback(() => {
        if (!isAuth0Loading && isAuthenticated) {
            dispatch(fetchDepartments({ pageNumber: page, pageSize }));
        }
    }, [page, isAuthenticated, isAuth0Loading, dispatch, pageSize]);

    useEffect(() => {
        fetchDepartmentsData();
    }, [fetchDepartmentsData]);

    const handleSuccess = useCallback(() => {
        setIsModalOpen(false);
        setDeptToEdit(undefined);
        fetchDepartmentsData();
    }, [fetchDepartmentsData]);

    const handleAddDepartment = () => {
        setDeptToEdit(undefined);
        setIsModalOpen(true);
    };

    const handleEditDepartment = (dept: GetDepartmentByIdQueryResponse) => {
        setDeptToEdit(dept);
        setIsModalOpen(true);
    };

    if (loading) return <PageLoader loadingText="Loading departments..." />;
    if (error) return <div className={commonStyles.errorMessage}>{error}</div>;

    return (
        <div className={commonStyles.container}>
            <div className={commonStyles.titleArea}>
                <h1 className={commonStyles.heading}>
                    <DepartmentIcon className={styles.headingIcon} />
                    Department Management
                </h1>
                <button className={commonStyles.createButton} onClick={handleAddDepartment}>Add New Department</button>
            </div>

            <div className={styles.departmentList}>
                <div className={commonStyles.listHeader}>
                    <span style={{ flex: 1 }}>Name</span>
                    <span style={{ flex: 8 }}>Description</span>
                    <span style={{ flex: 2, textAlign: 'center' }}>Actions</span>
                </div>
                {departments.map(dept => (
                    <div key={dept.id} className={styles.departmentItem}>
                        <span className={styles.name}>{dept.name}</span>
                        <span className={styles.description}>{dept.description}</span>
                        <div className={styles.actions}>
                            <button className={commonStyles.actionButton} onClick={() => handleEditDepartment(dept)}>Edit</button>
                        </div>
                    </div>
                ))}
            </div>

            <div className={commonStyles.pagination}>
                <button disabled={page === 1} onClick={() => dispatch(setDepartmentPage(page - 1))}>Previous</button>
                <span>Page {page} of {totalPages}</span>
                <button disabled={page >= totalPages} onClick={() => dispatch(setDepartmentPage(page + 1))}>Next</button>
            </div>

            <DepartmentFormModal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                onSuccess={handleSuccess}
                initialDepartment={deptToEdit}
            />
        </div>
    );
};

export default DepartmentPage;
