import React from 'react';
import type { UserDto } from '../entities/ims/dto/user_dto';
import { Role } from '../entities/ims/enums';
import styles from '../pages/UserManagementPage/UserManagementPage.module.css';
import {EditIcon} from "./common/Icons.tsx";

interface UserListItemProps {
  user: UserDto;
  onEdit: (user: UserDto) => void;
}

const getRoleDisplayName = (role: Role): string => {
  switch (role) {
    case Role.HumanResourcesManager: return "HR Manager";
    case Role.Intern: return "Intern";
    case Role.Mentor: return "Mentor";
    default: return `Unknown Role (${role})`;
  }
};

const UserListItem: React.FC<UserListItemProps> = ({ user, onEdit }) => {
  const getInitials = (firstName: string | null | undefined, lastName: string | null | undefined) => {
    const f = (firstName?.[0] || '');
    const l = (lastName?.[0] || '');
    return (f + l).toUpperCase() || 'U';
  };

  const initials = getInitials(user.firstname, user.lastname);

  return (
      <div key={user.id} className={styles.userItem}>

        <div className={styles.userAvatar}>
          <div className={styles.userInitials}>{initials}</div>
        </div>

        <div className={styles.userInfo}>
          <div className={styles.userName}>
            {user.firstname} {user.lastname}
            <span className={styles.userRoleText}>({getRoleDisplayName(user.role)})</span>
          </div>
          <div className={styles.userContact}>
            {user.email} | {user.phoneNumber}
          </div>
        </div>
        
        <button onClick={() => onEdit(user)} className={styles.editButton}>
          <EditIcon style={{ width: '16px', height: '16px', marginRight: '5px' }} />
          Edit
        </button>
      </div>
  );
};

export default UserListItem;
