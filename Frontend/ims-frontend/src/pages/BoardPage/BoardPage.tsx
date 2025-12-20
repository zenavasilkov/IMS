import React, {useEffect, useMemo, useState} from 'react';
import { useSelector } from 'react-redux';
import { useAppDispatch } from '../../components/useAppDispatch';
import type { RootState } from '../../store';
import {Role, TicketStatus} from '../../entities/ims/enums';
import PageLoader from '../../components/common/pageLoader/PageLoader';
import commonStyles from '../../components/common/commonStyles/commonPageStyles.module.css';
import styles from './BoardPage.module.css';
import type {TicketDto} from "../../entities/ims/dto/ticket_dto.ts";
import KanbanColumn from "../../components/kanbanBoardComponents/KanbanColumn.tsx";
import {fetchBoardData, updateTicketStatus, setBoardId} from "../../features/slices/boardKanbanSlice.ts";
import {DndContext, type DragEndEvent, PointerSensor, useDroppable, useSensor, useSensors} from "@dnd-kit/core";
import TicketFormModal from "../../components/modals/TicketFormModal.tsx";
import {useAuth0} from "@auth0/auth0-react";
import {fetchCurrentUserByEmail} from "../InternshipPage/InternshipPage.tsx";
import FeedbackModal from "../../components/modals/FeedbackModal.tsx";
import {fetchBoardByInternId} from "../../features/slices/mentorInternsSlice.ts";

const DroppableColumnWrapper: React.FC<any> = ({ children, id, ...props}) => {
    const { isOver, setNodeRef } = useDroppable({ id: id});
    const style = {opacity: isOver ? 0.8 : 1,  flexShrink: 0, ...props.style};
    return (<div ref={setNodeRef} style={style} className={styles.kanbanColumnWrapper}>{children}</div>);
};

const BoardPage: React.FC = () => {
    const dispatch = useAppDispatch();
    const { boardTitle, tickets, loading, error, boardId, boardDto } = useSelector((state: RootState) => state.boardKanban);
    const sensors = useSensors(useSensor(PointerSensor));
    const [isEditModalOpen, setIsEditModalOpen] = useState(false);
    const [ticketToEdit, setTicketToEdit] = useState<TicketDto | undefined>(undefined);
    const { user: auth0User, isAuthenticated } = useAuth0();
    const [isFeedbackModalOpen, setIsFeedbackModalOpen] = useState(false);
    const [ticketForFeedback, setTicketForFeedback] = useState<TicketDto | undefined>(undefined);
    const [currentUserId, setCurrentUserId] = useState<string | null>(null);
    const [userRole, setUserRole] = useState<Role | null>(null);
    const internId = boardDto?.createdToId;

    useEffect(() => {
        if (auth0User?.email && isAuthenticated) {
            const fetchCurrentUser = async () => {
                try {
                    const userDto = await fetchCurrentUserByEmail(auth0User.email as string);

                    setUserRole(userDto?.role as Role)
                    setCurrentUserId(userDto?.id as string);

                    if (userDto?.role === Role.Intern){
                        const board = await fetchBoardByInternId(userDto?.id as string);
                        dispatch(setBoardId(board?.id as string));
                    }
                } catch (e) {
                    console.error(e, "Failed to fetch current user DTO:");
                }
            };
            fetchCurrentUser();
        }

    }, [auth0User?.email, isAuthenticated]);

    const handleEditTicket = (ticket: TicketDto) => {
        setTicketToEdit(ticket);
        setIsEditModalOpen(true);
    };

    const handleTicketUpdateSuccess = () => {
        setIsEditModalOpen(false);
        setTicketToEdit(undefined);
        dispatch(fetchBoardData(boardId!));
    };

    useEffect(() => {
        if (boardId) {
            dispatch(fetchBoardData(boardId));
        }
    }, [boardId, boardTitle, dispatch]);

    const handleDragEnd = (event: DragEndEvent) => {
        const { over, active } = event;

        if (!over || !active) return;

        const ticketId = String(active.id);
        const newStatus = Number(over.id) as TicketStatus;

        if (newStatus === Number(active.data.current?.sortable.containerId)) return;

        dispatch(updateTicketStatus({ticketId, newStatus, boardId: boardId!}));
    };

    const ticketsByStatus = useMemo(() => {
        const groups: Record<TicketStatus, TicketDto[]> = {
            [TicketStatus.ToDo]: [],
            [TicketStatus.InProgress]: [],
            [TicketStatus.PullRequest]: [],
            [TicketStatus.Done]: [],
            [TicketStatus.Unassigned]: [],
        } as any;

        tickets.forEach(ticket => {
            const status = ticket.status as TicketStatus;
            if (groups[status]) {
                groups[status].push(ticket);
            }
        });
        return groups;
    }, [tickets]);

    const handleAddFeedback = (ticketId: string) => {
        const ticket = tickets.find(t => t.id === ticketId);

        if (ticket) {
            setTicketForFeedback(ticket);
            setIsFeedbackModalOpen(true);
        } else {
            console.error(`Ticket not found for ID: ${ticketId}. Cannot open feedback modal.`);
        }
    };

    const statusColumns = [
        { status: TicketStatus.ToDo, title: 'Todo', description: "This item hasn't been started" },
        { status: TicketStatus.InProgress, title: 'In Progress', description: 'This is actively being worked on' },
        { status: TicketStatus.PullRequest, title: 'Pull Request', description: 'This is ready for review' },
        { status: TicketStatus.Done, title: 'Done', description: 'This has been completed' },
    ];

    const handleTicketCreated = () => {
        dispatch(fetchBoardData(boardId!));
    };


    if (loading) return <PageLoader loadingText={`Loading board: ${boardId}...`} />;
    if (error) return <div className={commonStyles.errorMessage}>{error}</div>;

    return (
        <div className={styles.boardPageContainer}>
            <DndContext onDragEnd={handleDragEnd} sensors={sensors}>
                <div className={styles.kanbanBoardWrapper}>
                    <div className={styles.kanbanColumnsCentering}>
                        {statusColumns.map(col => (
                            <DroppableColumnWrapper key={col.status} id={String(col.status)}>
                                <KanbanColumn
                                    boardId={boardId as string}
                                    status={col.status}
                                    title={col.title}
                                    userRole={userRole}
                                    tickets={ticketsByStatus[col.status]}
                                    onTicketCreated={handleTicketCreated}
                                    onAddFeedback={handleAddFeedback}
                                    onEditTicket={handleEditTicket}
                                />
                            </DroppableColumnWrapper>
                        ))}
                    </div>
                </div>
            </DndContext>

            <TicketFormModal
                isOpen={isEditModalOpen}
                onClose={() => setIsEditModalOpen(false)}
                onSuccess={handleTicketUpdateSuccess}
                boardId={boardId!}
                initialStatus={ticketToEdit?.status || TicketStatus.ToDo}
                initialTicket={ticketToEdit}
            />

            <FeedbackModal
                isOpen={isFeedbackModalOpen}
                onClose={() => setIsFeedbackModalOpen(false)}
                ticketId={ticketForFeedback?.id || null}
                userRole={userRole}
                currentUserId={currentUserId}
                addressedToId={internId!}
            />
        </div>
    );
};

export default BoardPage;
