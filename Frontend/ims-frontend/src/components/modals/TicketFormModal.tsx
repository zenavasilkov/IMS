import React, { useState, useEffect } from 'react';
import {createNewTicket, updateTicketDetails} from '../../features/slices/boardKanbanSlice';
import ModalWrapper from './modalComponents/ModalWrapper.tsx';
import ModalField from './modalComponents/ModalField.tsx';
import styles from '../common/commonStyles/commonModalStyles.module.css'
import {TicketStatus} from "../../entities/ims/enums.ts";
import type {CreateTicketDto, TicketDto, UpdateTicketDto} from "../../entities/ims/dto/ticket_dto.ts";
import {useAppDispatch} from "../useAppDispatch.ts";
import {convertLocalToUTC} from "../../features/helpers/TimeConverter.ts";

interface TicketFormModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSuccess: () => void;
    boardId: string;
    initialStatus: TicketStatus;
    initialTicket?: TicketDto;
}

const getInitialFormData = (ticket?: TicketDto, initialStatus?: TicketStatus): CreateTicketDto | UpdateTicketDto => ({
    title: ticket?.title || '',
    description: ticket?.description || '',
    deadLine: ticket?.deadLine ?? '',
    status: ticket?.status || initialStatus || TicketStatus.ToDo,
    boardId: ticket?.boardId || '',
});

const TicketFormModal: React.FC<TicketFormModalProps> = ({ isOpen, onClose, onSuccess, boardId, initialStatus, initialTicket }) => {
    const dispatch = useAppDispatch();
    const isEditMode = !!initialTicket;
    const [formData, setFormData] = useState<CreateTicketDto | UpdateTicketDto>(getInitialFormData(initialTicket, initialStatus));
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (isOpen) {
            setFormData(getInitialFormData(initialTicket, initialStatus));
            setError(null);
        }
    }, [isOpen, initialTicket, initialStatus]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!formData.title || !formData.deadLine) return;

        setIsSubmitting(true);
        setError(null);

        try {
            if (isEditMode && initialTicket) {
                const updateCommand: UpdateTicketDto = {
                    title: formData.title,
                    description: formData.description,
                    deadLine: convertLocalToUTC(formData.deadLine),
                    status: initialTicket.status
                };
                await dispatch(updateTicketDetails({ id: initialTicket.id, command: updateCommand })).unwrap();
            } else {
                const createCommand: CreateTicketDto = {
                    title: formData.title,
                    description: formData.description,
                    deadLine: convertLocalToUTC(formData.deadLine),
                    status: initialStatus,
                    boardId: boardId,
                };
                await dispatch(createNewTicket(createCommand)).unwrap();
            }

            onSuccess();
        } catch (err: any) {
            setError(err.message || `Failed to ${isEditMode ? 'update' : 'create'} task.`);
        } finally {
            setIsSubmitting(false);
        }
    };

    if (!isOpen) return null;

    const titleText = isEditMode ? 'Edit Task' : `Create New Task (${TicketStatus[initialStatus]})`;
    const buttonText = isEditMode ? 'Save Changes' : 'Create Task';

    return (
        <ModalWrapper isOpen={isOpen} onClose={onClose} title={titleText} error={error}>
            <form onSubmit={handleSubmit} className={styles.form}>

                <ModalField label="Title" name="title" value={formData.title || ''} onChange={handleChange} required />

                <label>Description (Optional)<textarea
                    name="description"
                    value={formData.description || ''}
                    onChange={handleChange as any} rows={3}
                    className={styles.formInput}
                />
                </label>

                <ModalField label="Deadline" name="deadLine" type="date" value={formData.deadLine?.slice(0, 10) || ''} onChange={handleChange} required />

                <button type="submit" disabled={isSubmitting} className={styles.submitButton}>
                    {isSubmitting ? 'Saving...' : buttonText}
                </button>
            </form>
        </ModalWrapper>
    );
};
export default TicketFormModal;
