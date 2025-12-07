import React, { useEffect } from 'react';
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

const ManagementIcon = (props: any) => (
    <svg {...props} viewBox="0 0 24 24" fill="none">
        <path fill="currentColor" d="M19.4 12.99l.06-1.99a7.97 7.97 0 00-1.84-5.38l1.43-1.43a.99.99 0 000-1.4l-1.4-1.4a.99.99 0 00-1.4 0l-1.43 1.43a7.97 7.97 0 00-5.38-1.84L11 4.54a.99.99 0 00-1.99 0l-.06 1.99a7.97 7.97 0 00-5.38 1.84l-1.43-1.43a.99.99 0 00-1.4 0l-1.4 1.4a.99.99 0 000 1.4l1.43 1.43a7.97 7.97 0 00-1.84 5.38l-1.99.06a.99.99 0 000 1.99l1.99.06a7.97 7.97 0 001.84 5.38l-1.43 1.43a.99.99 0 000 1.4l1.4 1.4a.99.99 0 001.4 0l1.43-1.43a7.97 7.97 0 005.38 1.84l.06 1.99a.99.99 0 001.99 0l.06-1.99a7.97 7.97 0 005.38-1.84l1.43 1.43a.99.99 0 001.4 0l1.4-1.4a.99.99 0 000-1.4l-1.43-1.43a7.97 7.97 0 001.84-5.38l1.99-.06a.99.99 0 000-1.99l-1.99-.06zM12 16a4 4 0 110-8 4 4 0 010 8z"/>
    </svg>
);

const UserManagementPage: React.FC = () => {
    const { isAuthenticated, isLoading: isAuth0Loading } = useAuth0();
    const dispatch = useAppDispatch();
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

    const handleCreateUser = () => {
        console.log("Create New User clicked.");
        alert("Feature Coming Soon: User Creation Form!");
    };

    const handleEditUser = (user: UserDto) => {
        console.log("Edit User clicked for:", user.id, user.email);
        alert(`Feature Coming Soon: Editing user: ${user.firstname} ${user.lastname}`);
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
            <h1 className={styles.heading}>
                    <ManagementIcon className={styles.headingIcon} />
                    User Management Portal
            </h1>

            <UserFilterControls />

            <div className={styles.centeredButtonWrapper}> 
                <CreateUserButton onClick={handleCreateUser} />
            </div>
            
            <UserList users={users} onEditUser={handleEditUser} />
            
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
