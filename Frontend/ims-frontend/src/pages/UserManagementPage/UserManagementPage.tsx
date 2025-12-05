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

const UserManagementPage: React.FC = () => {
    const { isAuthenticated, isLoading: isAuth0Loading } = useAuth0();

    const dispatch = useAppDispatch();
    const { users, loading, error, page, totalPages, pageSize } = useSelector((state: RootState) => state.userManagement);

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
            dispatch(fetchUsers({ pageNumber: page, pageSize }));
        }
    }, [page, isAuthenticated, isAuth0Loading, dispatch, pageSize]);

    if (isAuth0Loading) return <PageLoader loadingText="Loading authentication..." />;
    if (loading) return <PageLoader loadingText="Loading user data..." />;
    if (error) return <div className={styles.error}>{error}</div>;

    return (
        <div className={styles.container}>
            <h1 className={styles.heading}>User Management Portal</h1>
            
            <CreateUserButton onClick={handleCreateUser} />
            
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
