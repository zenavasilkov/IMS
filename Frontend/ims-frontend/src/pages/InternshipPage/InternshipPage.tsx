import React, { useEffect, useState, useCallback } from 'react';
import { useSelector } from 'react-redux';
import { useAuth0 } from '@auth0/auth0-react';
import type { RootState } from '../../store';
import { useAppDispatch } from '../../components/useAppDispatch';
import PageLoader from '../../components/common/pageLoader/PageLoader';
import {fetchInternships, setInternshipPage} from '../../features/slices/internshipSlice';
import { InternshipStatus } from '../../entities/ims/enums';
import { InternshipIcon } from "../../components/common/Icons.tsx";
import PageLayout from '../../components/PageLayout';
import SimpleListHeader from '../../components/SimpleListHeader';
import commonStyles from '../../components/common/commonStyles/commonPageStyles.module.css';
import InternshipFormModal from "../../components/modals/InternshipFormModal.tsx";
import PaginationControls from "../../components/PaginationControls.tsx";
import type {UserDto, UserDtoPagedList} from "../../entities/ims/dto/user_dto.ts";
import {userService} from "../../api/services";
import type {FetchUsersParams} from "../../entities/ims/FetchParameters.ts";
import useMinLoadingTime from "../../hooks/useMinLoadingTime.ts";
import userStyles from '../UserManagementPage/UserManagementPage.module.css';
import styles from "./InternshipPage.module.css";

const INTERNSHIP_HEADER_CONFIG = [
    { label: 'Intern', flex: 3 },
    { label: 'Mentor', flex: 3 },
    { label: 'Dates', flex: 2, textAlign: 'center' as const },
    { label: 'Status', flex: 1.5, textAlign: 'center' as const },
    { label: 'Actions', flex: 1, textAlign: 'center' as const },
];

const getStatusData = (status: InternshipStatus) => {
    const text = InternshipStatus[status].replaceAll(/([A-Z])/g, ' $1').trim();
    let className = '';

    switch (status) {
        case InternshipStatus.NotStarted:
            className = 'statusNotStarted';
            break;
        case InternshipStatus.Ongoing:
            className = 'statusOngoing';
            break;
        case InternshipStatus.Completed:
            className = 'statusCompleted';
            break;
        case InternshipStatus.Cancelled:
            className = 'statusCancelled';
            break;
        default:
    }
    return { text, className };
};

const InternshipPage: React.FC = () => {
    const { isAuthenticated, isLoading: isAuth0Loading, user: auth0User } = useAuth0();
    const dispatch = useAppDispatch();
    const [allUsersMap, setAllUsersMap] = useState<Map<string, UserDto>>(new Map());
    const [hrManagerUser, setHrManagerUser] = useState<UserDto | undefined>(undefined);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [internshipToEdit, setInternshipToEdit] = useState<any>(undefined);
    const { internships, loading, error, page, totalPages, pageSize } = useSelector((state: RootState) => state.internship);
    const [isUserDataLoading, setIsUserDataLoading] = useState(true);
    const hrManagerId = hrManagerUser?.id;
    const showPageLoader = useMinLoadingTime(loading || isUserDataLoading);

    const fetchCurrentUserByEmail = async (email: string) => {
        const params: FetchUsersParams = {
            PageNumber: 1,
            PageSize: 1,
            Email: email
        };

        try {
            const result: UserDtoPagedList = await userService.getAllUsers(params);
            return result.items?.[0] || undefined;
        } catch (e) {
            console.error("API Error when searching user by email:", e);
            return undefined;
        }
    };

    const fetchInternshipsData = useCallback(() => {
        if (!isAuth0Loading && isAuthenticated && hrManagerId) {
            const params = {
                PageNumber: page,
                PageSize: pageSize
            };
            dispatch(fetchInternships(params));
        }
    }, [page, isAuthenticated, isAuth0Loading, dispatch, pageSize, hrManagerId]);

    useEffect(() => {
        if (auth0User?.email && isAuthenticated) {
            setIsUserDataLoading(true);

            const fetchCurrentUser = async () => {
                try {
                    const userDto = await fetchCurrentUserByEmail(auth0User.email as string);
                    setHrManagerUser(userDto);

                    await fetchAndMapAllUsers();
                } catch (e) {
                    console.error("Failed to fetch current user DTO:", e);
                } finally {
                    setIsUserDataLoading(false);
                }
            };
            fetchCurrentUser();
        }

    }, [auth0User?.email, isAuthenticated]);

    useEffect(() => {
        fetchInternshipsData();
    }, [fetchInternshipsData]);

    const fetchAndMapAllUsers = useCallback(async () => {
        try {
            const params: FetchUsersParams = { PageNumber: 1, PageSize: 1000 };
            const result: UserDtoPagedList = await userService.getAllUsers(params);

            const userMap = new Map<string, UserDto>();
            (result.items || []).forEach(user => {
                if (user.id) userMap.set(user.id, user);
            });
            setAllUsersMap(userMap);
        } catch (e) {
            console.error("Failed to fetch all users for map:", e);
        }
    }, []);

    const getUserDto = (id: string | undefined): UserDto | undefined => {
        return id ? allUsersMap.get(id) : undefined;
    };

    const handleSuccess = useCallback(() => { setIsModalOpen(false); setInternshipToEdit(undefined); fetchInternshipsData(); }, [fetchInternshipsData]);
    const handleAdd = () => { setInternshipToEdit(undefined); setIsModalOpen(true); };
    const handleEdit = (item: any) => { setInternshipToEdit(item); setIsModalOpen(true); };

    if (showPageLoader) return <PageLoader loadingText="Loading internships..." />;
    if (error) return <div className={commonStyles.errorMessage}>{error}</div>;

    return (
        <PageLayout
            title="Internship Management"
            Icon={InternshipIcon}
            iconColor="#ff8c00"
            createButton={<button className={commonStyles.createButton} onClick={handleAdd}>Create Internship</button>}
        >
            <div className={userStyles.userListContainer}>
                <SimpleListHeader columns={INTERNSHIP_HEADER_CONFIG} />

                {(internships || []).map(item => {
                    const intern = getUserDto(item.internId);
                    const mentor = getUserDto(item.mentorId);
                    const statusData = getStatusData(item.status);

                    return (
                        <div key={item.id} className={commonStyles.userItem}>
                            <div className={userStyles.userInfo} style={{ flex: 3 }}>
                                <span className={userStyles.userName}>{intern?.firstName} {intern?.lastName || 'N/A'}</span><br/>
                                <span className={userStyles.contact}>{intern?.email || 'N/A'}</span>
                            </div>

                            <div className={userStyles.userInfo} style={{ flex: 3 }}>
                                <span className={userStyles.userName}>{mentor?.firstName} {mentor?.lastName || 'N/A'}</span><br/>
                                <span className={userStyles.contact}>{mentor?.email || 'N/A'}</span>
                            </div>

                            <span style={{ flex: 2, textAlign: 'center' }}>{item.startDate.slice(0, 10)} - {item.endDate?.slice(0, 10) || 'N/A'}</span>
                            <span
                                style={{ flex: 1.5, textAlign: 'center' }}
                                className={styles[statusData.className]}
                            >
                                {statusData.text}
                            </span>
                            <div className={commonStyles.actions} style={{ flex: 1 }}>
                                <button className={commonStyles.actionButton} onClick={() => handleEdit(item)}>Edit</button>
                            </div>
                        </div>
                    );
                })}
            </div>

            <PaginationControls
                currentPage={page}
                totalPages={totalPages}
                onPreviousPage={() => dispatch(setInternshipPage(page - 1))}
                onNextPage={() => dispatch(setInternshipPage(page + 1))}
                hasContent={internships.length > 0}
            />

            <InternshipFormModal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                onSuccess={handleSuccess}
                initialInternship={internshipToEdit}
                currentUserDto={hrManagerUser}
            />
        </PageLayout>
    );
};

export default InternshipPage;
