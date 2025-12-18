import React, {useState} from 'react';
import type { TicketDto } from '../../entities/ims/dto/ticket_dto.ts';
import styles from './Kanban.module.css';
import {EditTicketIcon} from "../common/Icons.tsx";

interface KanbanCardProps {
    ticket: TicketDto;
    onAddFeedback: (ticketId: string) => void;
    onEdit: (ticket: TicketDto) => void;
    dragListeners?: any;
    dragAttributes?: any;
}

const DescriptionModal: React.FC<{ title: string, description: string, onClose: () => void }> = ({ title, description, onClose }) => (
    <div className={styles.detailModalOverlay} onClick={onClose}>
        <div className={styles.detailModalContent} onClick={e => e.stopPropagation()}>
            <h3 className={styles.detailModalTitle}>{title}</h3>
            <p className={styles.detailModalBody}>{description}</p>
            <button className={styles.detailModalCloseButton} onClick={onClose}>Close</button>
        </div>
    </div>
);

const KanbanCard: React.FC<KanbanCardProps> = ({ ticket, onAddFeedback, onEdit, dragAttributes, dragListeners}) => {
    const [isDescriptionModalOpen, setIsDescriptionModalOpen] = useState(false);

    return (
        <div className={styles.kanbanCard} {...dragAttributes}>
            <div className={styles.cardStatusAndTitle}>
                <div className={styles.cardTitle} {...dragListeners}>{ticket.title}</div>
                <button onClick={(e) => { e.stopPropagation(); onEdit(ticket); }} className={styles.editButton} >
                    <EditTicketIcon style={{ width: '20px', height: '20px' }} />
                </button>
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
                <DescriptionModal
                    title={ticket.title || 'Ticket Details'}
                    description={ticket.description}
                    onClose={() => setIsDescriptionModalOpen(false)}
                />
            )}
        </div>
    );
};

export default KanbanCard;
