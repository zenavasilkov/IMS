import React, {useState} from 'react';
import KanbanCard from './KanbanCard';
import styles from './Kanban.module.css';
import {TicketStatus} from "../../entities/ims/enums.ts";
import type {TicketDto} from "../../entities/ims/dto/ticket_dto.ts";
import TicketFormModal from "../modals/TicketFormModal.tsx";
import {useDraggable} from "@dnd-kit/core";

interface KanbanColumnProps {
    status: TicketStatus;
    title: string;
    tickets: TicketDto[];
    onAddFeedback: (ticketId: string) => void;
    boardId: string;
    onTicketCreated: () => void;
    onEditTicket: (ticket: TicketDto) => void;
}
const DraggableCardWrapper: React.FC<any> = ({ children, id}) => {
    const { attributes, listeners, setNodeRef, transform, isDragging } = useDraggable({id: id});

    const style = transform ? {
        transform: `translate3d(${transform.x}px, ${transform.y}px, 0)`,
        zIndex: isDragging ? 20 : 10,
        boxShadow: '0 5px 15px rgba(0, 0, 0, 0.7)',
    } : {
        transform: 'translate3d(0, 0, 0)',
        zIndex: 10,
    };

    return (
        <div
            ref={setNodeRef}
            style={style}
            className={isDragging ? styles.isDragging : ''}
        >
            {React.cloneElement(children, { dragListeners: listeners, dragAttributes: attributes })}
        </div>
    );
};

const KanbanColumn: React.FC<KanbanColumnProps> = ({ status, title, tickets, onAddFeedback, onTicketCreated, boardId, onEditTicket }) => {
    const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);

    const getStatusClass = (status: TicketStatus) => {
        switch (status) {
            case TicketStatus.ToDo: return styles.statusTodo;
            case TicketStatus.InProgress: return styles.statusInProgress;
            case TicketStatus.PullRequest: return styles.statusPullRequest;
            case TicketStatus.Done: return styles.statusDone;
            default: return '';
        }
    };

    const handleAddItemClick = () => {
        setIsCreateModalOpen(true);
    };

    return (
        <div className={styles.kanbanColumn}>
            <div className={`${styles.columnHeader} ${getStatusClass(status)}`}>
                <div className={styles.columnTitle}>
                    {title}
                    <span className={styles.ticketCount}>{tickets.length}</span>
                </div>
                <button className={styles.headerMenuButton}>...</button>
            </div>

            <div className={styles.columnBody}>
                {/* FIX: Map list items to DraggableCardWrapper */}
                {tickets.map((ticket) => (
                    <DraggableCardWrapper
                        key={ticket.id}
                        id={ticket.id}
                        onAddFeedback={onAddFeedback}
                    >
                        {/* Render the original KanbanCard inside the wrapper */}
                        <KanbanCard ticket={ticket} onAddFeedback={onAddFeedback} onEdit={onEditTicket}/>
                    </DraggableCardWrapper>
                ))}

                <button className={styles.addItemButton} onClick={handleAddItemClick}>+ Add item</button>
            </div>

            <TicketFormModal
                isOpen={isCreateModalOpen}
                onClose={() => setIsCreateModalOpen(false)}
                onSuccess={onTicketCreated}
                boardId={boardId}
                initialStatus={status}
            />
        </div>
    );
};

export default KanbanColumn;
