import React, { useState, useEffect } from 'react';
import { userService } from '../api/services';
import type { UserDto } from '../entities/ims/dto/user_dto';
import styles from './common/commonStyles/commonModalStyles.module.css';
import type {Role} from "../entities/ims/enums.ts";

interface UserSearchSelectProps {
    label: string | null;
    onSelect: (id: string) => void;
    required: boolean;
    currentUserId: string;
    filterRole?: Role;
    disabled?: boolean;
    placeholder?: string;
    displayFullName?: boolean;
    style?: React.CSSProperties;
}

const UserSearchSelect: React.FC<UserSearchSelectProps> = ({ label, onSelect, required,
    currentUserId, filterRole, disabled, placeholder, displayFullName, style}) => {

    const [searchTerm, setSearchTerm] = useState('');
    const [searchResults, setSearchResults] = useState<UserDto[]>([]);
    const [isSearching, setIsSearching] = useState(false);
    const [selectedUserName, setSelectedUserName] = useState('');

    useEffect(() => {
        const handler = setTimeout(() => {
            if (searchTerm.length >= 3) {
                const searchUsers = async () => {
                    setIsSearching(true);
                    try {
                        const params = { PageNumber: 1, PageSize: 10, Email: searchTerm, Role: filterRole };
                        const result = await userService.getAllUsers(params);
                        setSearchResults(result.items || []);
                    } catch (e) {
                        setSearchResults([]);
                        console.error(e, "Error during user selecting.")
                    } finally {
                        setIsSearching(false);
                    }
                };
                searchUsers();
            } else {
                setSearchResults([]);
            }
        }, 1000);
        return () => clearTimeout(handler);
    }, [searchTerm, filterRole]);

    useEffect(() => {
        if (currentUserId && !selectedUserName) {
            const fetchUserName = async () => {
                try {
                    const user = await userService.getUserById(currentUserId);

                    const fullName = displayFullName ? `${user.firstName} ${user.lastName}` : '';

                    if (user) setSelectedUserName(`${fullName} ${user.email}`);
                } catch(e) {
                    console.error("Error during user fetching", e);
                    setSelectedUserName('User Not Found');
                }
            }
            fetchUserName();
        } else if (!currentUserId) {
            setSelectedUserName('');
        }
    }, [currentUserId, selectedUserName]);

    const handleSelect = (user: UserDto) => {
        const fullName = displayFullName ? `${user.firstName} ${user.lastName}` : '';
        setSelectedUserName(`${fullName} ${user.email}`);
        onSelect(user.id);
        setSearchTerm('');
    };

    return (
        <label style={style}>
            {label}
            <div className={styles.searchSelectWrapper}>
                <input
                    type="text"
                    value={searchTerm || selectedUserName}
                    onChange={(e) => {
                        setSearchTerm(e.target.value);
                        setSelectedUserName('');
                        if (!e.target.value) onSelect('');
                    }}
                    required={required}
                    placeholder={ placeholder || `Search by email...`}
                    disabled={disabled || isSearching}
                    className={styles.formInput}

                />

                {isSearching && <div className={styles.spinner}></div>}

                {searchResults.length > 0 && (
                    <div className={styles.searchResultsDropdown}>
                        {searchResults.map(user => (
                            <div
                                key={user.id}
                                className={styles.searchResultItem}
                                onClick={() => handleSelect(user)}
                            >
                                { (displayFullName === true) && <span >{user.firstName} {user.lastName}</span>}
                                <span className={styles.searchResultEmail}>{ ' ' + user.email}</span>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </label>
    );
};

export default UserSearchSelect;
