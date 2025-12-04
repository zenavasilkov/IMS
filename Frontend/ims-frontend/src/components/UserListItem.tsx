import React from 'react';
import type { UserDto } from '../entities/ims/dto/user_dto';
import { Role } from '../entities/ims/enums';
import styles from '../pages/UserManagementPage/UserManagementPage.module.css';

interface UserListItemProps {
  user: UserDto;
  onEdit: (user: UserDto) => void;
}

const getRoleDisplayName = (role: Role): string => {
  switch (role) {
    case Role.HRManager: return "HR Manager";
    case Role.Intern: return "Intern";
    case Role.Mentor: return "Mentor";
    default: return `Unknown Role (${role})`;
  }
};

const UserListItem: React.FC<UserListItemProps> = ({ user, onEdit }) => {
  return (
    <div key={user.id} className={styles.userItem}>
      <div className={styles.userInfo}>
        <div className={styles.userName}>{user.firstname} {user.lastname} ({getRoleDisplayName(user.role)})</div>
        <div className={styles.userContact}>{user.email} | {user.phoneNumber}</div>
      </div>
      <button onClick={() => onEdit(user)} className={styles.button}>Edit</button>
    </div>
  );
};

export default UserListItem;
