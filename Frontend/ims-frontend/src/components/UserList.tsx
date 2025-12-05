import React from 'react';
import type { UserDto } from '../entities/ims/dto/user_dto';
import styles from '../pages/UserManagementPage/UserManagementPage.module.css';
import UserListItem from './UserListItem';

interface UserListProps {
  users: UserDto[];
  onEditUser: (user: UserDto) => void;
}

const UserList: React.FC<UserListProps> = ({ users, onEditUser }) => {
  return (
    <div className={styles.userListContainer}>
      {users.length === 0 ? (
        <div className={styles.userItem} style={{ justifyContent: 'center', color: '#a0a0a0' }}>
          No users found.
        </div>
      ) : (
        users.map(user => (
          <UserListItem key={user.id} user={user} onEdit={onEditUser} />
        ))
      )}
    </div>
  );
};

export default UserList;
