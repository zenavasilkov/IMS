import React, { useMemo } from 'react';
import { useSelector } from 'react-redux';
import commonStyles from '../common/commonStyles/commonPageStyles.module.css';
import {useAppDispatch} from "../useAppDispatch.ts";
import type {RootState} from "../../store.ts";
import {InternshipStatus, Role} from "../../entities/ims/enums.ts";
import UserSearchSelect from "../UserSearchSelect.tsx";
import {
    resetInternshipFilters,
    setFilterInternId,
    setFilterMentorId, setFilterStartedAfter, setFilterStartedBefore,
    setFilterStatus
} from "../../features/slices/internshipSlice.ts";

const InternshipFilterControls: React.FC = () => {
    const dispatch = useAppDispatch();
    const { filterInternId, filterMentorId, filterStatus, filterStartedAfter, filterStartedBefore } = useSelector((state: RootState) => state.internship);

    const statusOptions = useMemo(() => {
        const options = [{ value: '', label: 'Status: All' }];
        Object.keys(InternshipStatus).filter(key => Number.isNaN(Number(key))).forEach(key => {
            options.push({
                value: String(InternshipStatus[key as keyof typeof InternshipStatus]),
                label: key.replaceAll(/([A-Z])/g, ' $1').trim()
            });
        });
        return options;
    }, []);

    const handleResetClick = () => {
        dispatch(resetInternshipFilters());
    };

    return (
        <div className={commonStyles.filterBar}>

            <UserSearchSelect
                label={null}
                onSelect={(id) => dispatch(setFilterInternId(id))}
                currentUserId={filterInternId}
                filterRole={Role.Intern}
                required={false}
                placeholder={"Search by intern email..."}
                style={{ flexGrow: 1 }}
            />

            <UserSearchSelect
                label={null}
                onSelect={(id) => dispatch(setFilterMentorId(id))}
                currentUserId={filterMentorId}
                filterRole={Role.Mentor}
                required={false}
                placeholder={"Search by mentor email..."}
                style={{ flexGrow: 1 }}
            />

            <select
                className={commonStyles.selectInput}
                value={String(filterStatus)}
                onChange={(e) => {
                    const selectedValue = e.target.value;

                    if (selectedValue === '') {
                        dispatch(setFilterStatus(''));
                    } else {
                        dispatch(setFilterStatus(Number(selectedValue) as InternshipStatus));
                    }
                }}
            >
                {statusOptions.map(option => (
                    <option key={option.value} value={option.value}>{option.label}</option>
                ))}
            </select>

            <input
                type="date"
                className={commonStyles.searchInput}
                value={filterStartedAfter}
                onChange={(e) => dispatch(setFilterStartedAfter(e.target.value))}
                placeholder="Start Date After"
                style={{ flexGrow: 0, width: 'auto' }}
            />

            <input
                type="date"
                className={commonStyles.searchInput}
                value={filterStartedBefore}
                onChange={(e) => dispatch(setFilterStartedBefore(e.target.value))}
                placeholder="Start Date Before"
                style={{ flexGrow: 0, width: 'auto' }}
            />

            <button className={commonStyles.actionButton} onClick={handleResetClick}>Reset</button>
        </div>
    );
};

export default InternshipFilterControls;