import React, { useMemo, useState, useEffect } from "react";
import { useSelector } from "react-redux";
import { useAppDispatch } from "./useAppDispatch.ts";
import type {RootState} from "../store.ts";
import {Role, UserSortingParameter} from "../entities/ims/enums.ts";
import styles from '../pages/UserManagementPage/UserManagementPage.module.css';
import commonStyles from './common/commonStyles/commonPageStyles.module.css'
import { setFilterFirstName, setFilterLastName, setFilterRole, setSortParameter, resetFilters } from '../features/slices/userManagementSlice.ts';

const UserFilterControls : React.FC = () => {
    const dispatch = useAppDispatch();
    const { filterFirstName, filterLastName, filterRole, sortParameter } = useSelector((state: RootState) => state.userManagement);
    const [localFirstName, setLocalFirstName] = useState(filterFirstName);
    const [localLastName, setLocalLastName] = useState(filterLastName);
    const [localRole, setLocalRole] = useState(filterRole);
    const [localSort, setLocalSort] = useState(sortParameter);

    useEffect(() => {
        setLocalFirstName(filterFirstName);
        setLocalLastName(filterLastName);
        setLocalRole(filterRole);
        setLocalSort(sortParameter);
    }, [filterFirstName, filterLastName, filterRole, sortParameter]);

    const handleApplyFilters = () => {
        dispatch(setFilterFirstName(localFirstName));
        dispatch(setFilterLastName(localLastName));
        dispatch(setFilterRole(localRole));
        dispatch(setSortParameter(localSort));
    };

    const handleResetFilters = () => {
        dispatch(resetFilters());
    };

    const roleOptions = useMemo(() => {
        const options = [{ value: '', label: 'Search by: Role' }];
        
        type RoleKeys = keyof typeof Role;
        
        Object.keys(Role).forEach(key  => {
            if (Number.isNaN(Number(key))) {
                options.push({
                    value: String(Role[key as RoleKeys]),
                    label: key.replaceAll(/([A-Z])/g, ' $1').trim()
                });
            }
        });
        return options;
    }, []);

    const sortOptions = useMemo(() => [
        { value: UserSortingParameter.None, label: 'Sort by: Parameter' },
        { value: UserSortingParameter.AscendingLastName, label: 'Last Name (A-Z)' },
        { value: UserSortingParameter.DescendingLastName, label: 'Last Name (Z-A)' },
        { value: UserSortingParameter.AscendingFirstName, label: 'First Name (A-Z)' },
        { value: UserSortingParameter.DescendingFirstName, label: 'First Name (Z-A)' },
        { value: UserSortingParameter.AscendingRole, label: 'Role (Asc)' },
        { value: UserSortingParameter.DescendingRole, label: 'Role (Desc)' },
    ].map(option => ({
        value: String(option.value),
        label: option.label
    })), []);

    return (
        <div className={commonStyles.filterBar}>
            <input
                type="text"
                className={commonStyles.searchInput}
                placeholder="Search by First Name"
                value={localFirstName}
                onChange={(e) => setLocalFirstName(e.target.value)}
            />

            <input
                type="text"
                className={commonStyles.searchInput}
                placeholder="Search by Last Name"
                value={localLastName}
                onChange={(e) => setLocalLastName(e.target.value)}
                style={{ flexGrow: 0.5 }}
            />

            <select
                className={styles.selectInput}
                value={localRole || ''}
                onChange={(e) => setLocalRole(e.target.value as Role | '')}
            >
                {roleOptions.map(option => (
                    <option
                        key={option.value}
                        value={option.value}
                    >
                        {option.label}
                    </option>
                ))}
            </select>

            <select
                className={styles.selectInput}
                value={localSort}
                onChange={(e) => setLocalSort(Number(e.target.value) as UserSortingParameter)}
            >
                {sortOptions.map(option => (
                    <option key={option.value} value={option.value}>{option.label}</option>
                ))}
            </select>
 
            <button
                className={styles.resetButton}
                onClick={handleApplyFilters}
            >
                Apply
            </button>
 
            <button
                className={styles.resetButton}
                onClick={() => handleResetFilters()}
            >
                Reset
            </button>
        </div>
    );
};

export default UserFilterControls;
