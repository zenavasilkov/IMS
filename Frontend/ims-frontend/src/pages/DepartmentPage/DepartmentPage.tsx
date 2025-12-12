import React, {useCallback, useEffect, useState} from 'react';
import { useSelector } from 'react-redux';
import { useAuth0 } from '@auth0/auth0-react';
import type { RootState } from '../../store';
import { useAppDispatch } from '../../components/useAppDispatch';
import PageLoader from '../../components/common/pageLoader/PageLoader';
import styles from './DepartmentPage.module.css';
import commonStyles from '../../components/common/commonStyles/commonPageStyles.module.css'
import {fetchDepartments, setDepartmentPage} from "../../features/slices/departmentSlice.ts";
import {DepartmentIcon} from "../../components/common/Icons.tsx";
import type {GetDepartmentByIdQueryResponse} from "../../entities/recruitment/dto/department_dto.ts";
import DepartmentFormModal from "../../components/modals/DepartmentModalForm.tsx";
import PaginationControls from "../../components/PaginationControls.tsx";
import PageLayout from "../../components/PageLayout.tsx";
import SimpleListHeader from "../../components/SimpleListHeader.tsx";

const DEPARTMENT_HEADER_CONFIG = [
    { label: 'Name', flex: 3 },
    { label: 'Description', flex: 6, textAlign: 'left' as const },
    { label: 'Actions', flex: 2, textAlign: 'center' as const },
];

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

    return(
        <PageLayout
            title="Department Management"
            Icon={DepartmentIcon}
            iconColor="#ffc107"
            createButton={<button className={commonStyles.createButton} onClick={handleAddDepartment}>Add New Department</button>}
        >
            <div className={styles.departmentList}>
                <SimpleListHeader columns={DEPARTMENT_HEADER_CONFIG} />
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

            <PaginationControls
                currentPage={page}
                totalPages={totalPages}
                onPreviousPage={() => dispatch(setDepartmentPage(page - 1))}
                onNextPage={() => dispatch(setDepartmentPage(page + 1))}
                hasContent={departments.length > 0}
            />

            <DepartmentFormModal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                onSuccess={handleSuccess}
                initialDepartment={deptToEdit}
            />
        </PageLayout>
    );
};

export default DepartmentPage;
