import React, { useState, useEffect } from 'react';
import type {InternWithInternship} from "../../features/slices/mentorInternsSlice.ts";
import type {BoardDto, CreateBoardDto, UpdateBoardDto} from "../../entities/ims/dto/board_dto.ts";
import {boardService} from "../../api/services";
import ModalWrapper from "./modalComponents/ModalWrapper.tsx";
import ModalField from "./modalComponents/ModalField.tsx";
import styles from '../common/commonStyles/commonModalStyles.module.css'

interface BoardFormModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSuccess: () => void;
    mentorId: string | undefined;
    intern: InternWithInternship | undefined;
    initialBoard?: BoardDto;
}

const getInitialFormData = (board?: BoardDto) => ({
    title: board?.title || '',
    description: board?.description || '',
});

const BoardFormModal: React.FC<BoardFormModalProps> = ({ isOpen, onClose, onSuccess, mentorId, intern, initialBoard }) => {
    const isEditMode = !!initialBoard;
    const [formData, setFormData] = useState(getInitialFormData(initialBoard));
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (isOpen) {
            setFormData(getInitialFormData(initialBoard));
            setError(null);
        }
    }, [isOpen, initialBoard]);


    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        setFormData(prev => ({ ...prev, [e.target.name]: e.target.value }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!mentorId || !intern?.id) {
            return setError("Error: Mentor or Intern context is missing.");
        }

        setIsSubmitting(true);
        setError(null);

        try {
            if (isEditMode && initialBoard) {
                const updateCommand: UpdateBoardDto = {
                    title: formData.title,
                    description: formData.description,
                };
                await boardService.updateBoard(initialBoard.id, updateCommand);

            } else {
                const createCommand: CreateBoardDto = {
                    title: formData.title,
                    description: formData.description,
                    createdById: mentorId,
                    createdToId: intern.id,
                };
                await boardService.createBoard(createCommand);
            }

            onSuccess();
            onClose();
        } catch (err: any) {
            console.error("Board submission failed:", err);
            setError(err.response?.data?.message || `Failed to ${isEditMode ? 'update' : 'create'} Kanban board.`);
        } finally {
            setIsSubmitting(false);
        }
    };

    if (!isOpen || (!isEditMode && !intern)) return null;

    const titleText = isEditMode ? `Edit Board: ${initialBoard?.title}` : `Create Board for ${intern!.firstName} ${intern!.lastName}`;

    return (
        <ModalWrapper
            isOpen={isOpen}
            onClose={onClose}
            title={titleText}
            error={error}
        >
            <form onSubmit={handleSubmit} className={styles.form}>
                <ModalField label="Board Title" name="title" value={formData.title || ''} onChange={handleChange} required />

                <label>Board Description (Optional)<textarea
                    name="description"
                    value={formData.description || ''}
                    onChange={handleChange as any}
                    rows={4}
                    className={styles.formInput}
                /></label>


                <button type="submit" disabled={isSubmitting || !mentorId} className={styles.submitButton}>
                    {isSubmitting ? 'Saving...' : (isEditMode ? 'Save Changes' : 'Create Board')}
                </button>
            </form>
        </ModalWrapper>
    );
};

export default BoardFormModal;
