import React, {useCallback, useEffect, useState} from 'react';
import { useSelector } from 'react-redux';
import { useAuth0 } from '@auth0/auth0-react';
import type { RootState } from '../../store';
import { useAppDispatch } from '../../components/useAppDispatch';
import PageLoader from '../../components/common/pageLoader/PageLoader';
import commonStyles from '../../components/common/commonStyles/commonPageStyles.module.css';
import PageLayout from '../../components/PageLayout';
import SimpleListHeader from '../../components/SimpleListHeader';
import {
    fetchBoardByInternId,
    fetchMentorInterns,
    type InternWithInternship,
    setMentorId
} from "../../features/slices/mentorInternsSlice.ts";
import {MentorIcon} from "../../components/common/Icons.tsx";
import type {UserDto} from "../../entities/ims/dto/user_dto.ts";
import type {FetchUsersParams} from "../../entities/ims/FetchParameters.ts";
import {userService} from "../../api/services";
import type {BoardDto} from "../../entities/ims/dto/board_dto.ts";
import BoardFormModal from "../../components/modals/BoardFormModal.tsx";
import {getStatusData} from "../../features/helpers/GetInternshipStatusData.ts";
import styles from "../InternshipPage/InternshipPage.module.css";
import {setBoardId} from "../../features/slices/boardKanbanSlice.ts";
import {PORTALS} from "../../components/portalSwitcher/PortalSwitcher.tsx";
import {setAppActivePortal} from "../../features/slices/appSlice.ts";

const fetchUserDtoByEmail = async (email: string) => {
    const params: FetchUsersParams = { PageNumber: 1, PageSize: 1, Email: email };
    try {
        const result = await userService.getAllUsers(params);
        return result.items?.[0] || undefined;
    } catch (e) {
        console.error("API Error when searching user by email:", e);
        return undefined;
    }
};

const INTERN_HEADER_CONFIG = [
    { label: 'Intern Details', flex: 3.5 },
    { label: 'Internship Status', flex: 1.5, textAlign: 'center' as const },
    { label: 'Board Actions', flex: 2, textAlign: 'center' as const },
];

const MentorPage: React.FC = () => {
    const { isAuthenticated, user: auth0User, isLoading: isAuth0Loading } = useAuth0();
    const [isBoardModalOpen, setIsBoardModalOpen] = useState(false);
    const [boardToEdit, setBoardToEdit] = useState<BoardDto | undefined>(undefined);
    const [internToCreateBoardFor, setInternToCreateBoardFor] = useState<InternWithInternship | undefined>(undefined);
    const dispatch = useAppDispatch();
    const { interns, loading, error } = useSelector((state: RootState) => state.mentorInterns);
    const [mentorUser, setMentorUser] = useState<UserDto | undefined>(undefined);
    const loggedInMentorId = mentorUser?.id;

    useEffect(() => {
        if (isAuthenticated && loggedInMentorId && !isAuth0Loading) {
            dispatch(fetchMentorInterns(loggedInMentorId));
            dispatch(setMentorId(loggedInMentorId));
        }
    }, [isAuthenticated, loggedInMentorId, isAuth0Loading, dispatch]);

    useEffect(() => {
        if (auth0User?.email && isAuthenticated) {
            const fetchMentor = async () => {
                const userDto = await fetchUserDtoByEmail(auth0User.email as string);
                setMentorUser(userDto);
            };
            fetchMentor();
        }
    }, [isAuthenticated, auth0User?.email]);

    const handleOpenBoard = async (intern: InternWithInternship) => {
        const board = await fetchBoardByInternId(intern.id);

        if (board) {
            dispatch(setBoardId(board.id));

            dispatch(setAppActivePortal(PORTALS.BOARD_VIEW));
        }
    };

    const handleCreateBoard = (intern: InternWithInternship) => {
        setBoardToEdit(undefined);
        setInternToCreateBoardFor(intern);
        setIsBoardModalOpen(true);
    };

    const handleEditBoard = async (intern: InternWithInternship) => {
        const board = await fetchBoardByInternId(intern.id);

        if (board) {
            setBoardToEdit(board);
            setInternToCreateBoardFor(intern);
            setIsBoardModalOpen(true);
        } else {
            console.error(`Board not found for intern ID: ${intern.id}`);
        }
    };

    const handleBoardCreationSuccess = useCallback(() => {
        setIsBoardModalOpen(false);
        setInternToCreateBoardFor(undefined);
        dispatch(fetchMentorInterns(loggedInMentorId!));
    }, [loggedInMentorId, dispatch]);

    if (loading) return <PageLoader loadingText="Loading assigned interns..." />;
    if (error) return <div className={commonStyles.errorMessage}>{error}</div>;

    return (
        <PageLayout
            title="Mentor Portal"
            Icon={MentorIcon}
            iconColor="#ff8c00"
            createButton={null}
        >
            <div className={commonStyles.listContainer}>
                <SimpleListHeader columns={INTERN_HEADER_CONFIG} />

                {interns.map(intern => {
                    const statusData = getStatusData(intern.assignedInternship.status);

                    return (
                        <div key={intern.id} className={commonStyles.userItem}>
                            <div className={commonStyles.userInfo} style={{ flex: 3.5 }}>
                                <span className={commonStyles.userName}>{intern.firstName} {intern.lastName} </span>
                                <span className={commonStyles.userContact}>{intern.email} | {intern.phoneNumber}</span>
                            </div>

                            <span className={styles[statusData.className]} style={{ flex: 1.5 }} >{statusData.text}</span>

                            <div className={commonStyles.actions} style={{ flex: 2, justifyContent: 'flex-end' }}>
                                {intern.hasBoard ? (
                                    <>
                                        <button className={commonStyles.actionButton} onClick={() => handleOpenBoard(intern)}>Open Board</button>
                                        <button className={commonStyles.actionButton} onClick={() => handleEditBoard(intern)}>Edit Board</button>
                                    </>
                                ) : (
                                    <button className={commonStyles.actionButton} onClick={() => handleCreateBoard(intern)}>Create Board</button>
                                )}
                            </div>
                        </div>
                    )
                })}
            </div>

            <BoardFormModal
                isOpen={isBoardModalOpen}
                onClose={() => setIsBoardModalOpen(false)}
                onSuccess={handleBoardCreationSuccess}
                mentorId={loggedInMentorId}
                intern={internToCreateBoardFor}
                initialBoard={boardToEdit}
            />

        </PageLayout>
    );
};

export default MentorPage;
