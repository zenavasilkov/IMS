import React, {useState} from 'react';
import type { TicketDto } from '../../entities/ims/dto/ticket_dto.ts';
import styles from './Kanban.module.css';
import {EditTicketIcon} from "../common/Icons.tsx";
import ReactDOM from 'react-dom';
import {Role} from "../../entities/ims/enums.ts";

interface KanbanCardProps {
    ticket: TicketDto;
    onAddFeedback: (ticketId: string) => void;
    onEdit: (ticket: TicketDto) => void;
    userRole: Role | null;
    dragListeners?: any;
    dragAttributes?: any;
}

const DescriptionModal: React.FC<{ title: string, description: string, onClose: () => void }> = ({ title, description, onClose }) => (
    <div className={styles.detailModalOverlay} onClick={onClose}>
        <div className={styles.detailModalContent} onClick={e => e.stopPropagation()}>
            <h3 className={styles.detailModalTitle}>{title}</h3>
            <p className={styles.detailModalBody}>{description}</p>
            <div className={styles.modalFooter}><button className={styles.detailModalCloseButton} onClick={onClose}>Close</button></div>
        </div>
    </div>
);

const KanbanCard: React.FC<KanbanCardProps> = ({ ticket, onAddFeedback, onEdit, dragAttributes, dragListeners, userRole}) => {
    const [isDescriptionModalOpen, setIsDescriptionModalOpen] = useState(false);
    const isMentor = userRole === Role.Mentor;

    return (
        <div className={styles.kanbanCard} {...dragAttributes}>
            <div className={styles.cardStatusAndTitle}>
                <div className={styles.cardTitle} {...dragListeners}>{ticket.title}</div>
                {isMentor && <button onClick={(e) => { e.stopPropagation(); onEdit(ticket); }} className={styles.editButton}>
                    <EditTicketIcon style={{ width: '20px', height: '20px' }} />
                </button>}
                <button
                    onClick={(e) => { e.stopPropagation(); onAddFeedback(ticket.id); }}
                    className={styles.feedbackButton}
                >
                    ...
                </button>
            </div>
            {ticket.description && (
                <div
                    className={styles.cardDescription}
                    onClick={(e) => { e.stopPropagation(); setIsDescriptionModalOpen(true); }}
                >
                    {ticket.description}
                </div>
            )}
            <div className={styles.cardDetail}>Deadline: {new Date(ticket.deadLine).toLocaleDateString()}</div>

            {isDescriptionModalOpen && ticket.description && (
                ReactDOM.createPortal(
                    <DescriptionModal
                        title={ticket.title || 'Ticket Details'}
                        description={ticket.description}
                        onClose={() => setIsDescriptionModalOpen(false)}
                    />,
                    document.body
                )
            )}
        </div>
    );
};

export default KanbanCard;
