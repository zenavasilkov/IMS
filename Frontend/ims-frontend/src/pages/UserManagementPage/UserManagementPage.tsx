import React, {useEffect, useState} from 'react';
import type { UserDto } from '../../entities/ims/dto/user_dto';
import styles from './UserManagementPage.module.css';
import { useAuth0 } from '@auth0/auth0-react';
import CreateUserButton from '../../components/CreateUserButton';
import UserList from '../../components/UserList';
import PaginationControls from '../../components/PaginationControls';
import { useSelector } from 'react-redux';
import { fetchUsers, setPage } from '../../features/userManagement/userManagementSlice';
import type { RootState } from '../../store';
import { useAppDispatch } from '../../components/useAppDispatch';
import PageLoader from '../../components/common/pageLoader/PageLoader';
import UserFilterControls from "../../components/UserFilterControl.tsx";
import useMinLoadingTime from "../../hooks/useMinLoadingTime.ts";
import UserFormModal from "../../components/common/userFormModal/UserFormModal.tsx";

const UserManagementPage: React.FC = () => {
    const { isAuthenticated, isLoading: isAuth0Loading } = useAuth0();
    const dispatch = useAppDispatch();
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [userToEdit, setUserToEdit] = useState<UserDto | undefined>(undefined);
    
    const { 
        users,
        loading,
        error,
        page,
        totalPages,
        pageSize,
        filterFirstName,
        filterLastName,
        filterRole,
        sortParameter
    } = useSelector((state: RootState) => state.userManagement);

    const handleSuccess = () => {
        setIsModalOpen(false);
        setUserToEdit(undefined);
        
        dispatch(fetchUsers({
            pageNumber: page,
            pageSize: pageSize,
            firstName: filterFirstName,
            lastName: filterLastName,
            role: filterRole || undefined,
            sorter: sortParameter
        }));
    };

    const handleCreateUser = () => {
        setUserToEdit(undefined);
        setIsModalOpen(true);
    };

    const handleEditUser = (user: UserDto) => {
        setUserToEdit(user);
        setIsModalOpen(true);
    };

    useEffect(() => {
        if (!isAuth0Loading && isAuthenticated) {
            const params = {
                pageNumber: page,
                pageSize: pageSize,
                firstName: filterFirstName || undefined,
                lastName: filterLastName || undefined,
                role: filterRole || undefined,
                sorter: sortParameter
            };

            dispatch(fetchUsers(params));
        }
    }, [
        page,
        isAuthenticated,
        isAuth0Loading,
        dispatch,
        pageSize,
        filterFirstName,
        filterLastName,
        filterRole,
        sortParameter
    ]);

    const showDataLoading = useMinLoadingTime(loading, 300);
    
    if (isAuth0Loading) return <PageLoader loadingText="Loading authentication..." />;
    if (showDataLoading) return <PageLoader loadingText="Loading user data..." />;
    if (error) return <div className={styles.error}>{error}</div>;

    return (
        <div className={styles.container}>
            <div className={styles.centeredButtonWrapper}>
                <UserFilterControls />
                <CreateUserButton onClick={handleCreateUser} />
            </div>
            
            <UserList users={users} onEditUser={handleEditUser} />

            <UserFormModal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                onSuccess={handleSuccess}
                initialUser={userToEdit}
            />
            
            <PaginationControls
              currentPage={page}
              totalPages={totalPages}
              onPreviousPage={() => dispatch(setPage(page - 1))}
              onNextPage={() => dispatch(setPage(page + 1))}
              hasUsers={users.length > 0}
            />
        </div>
    );
};

export default UserManagementPage;
