import React, { useState, useEffect } from 'react';
import type { FeedbackDto, CreateFeedbackDto } from '../../entities/ims/dto/feedback_dto';
import { fetchFeedbackByTicket, addFeedbackToTicket, updateFeedbackAction } from '../../features/slices/boardKanbanSlice';
import styles from './FeedbackModal.module.css';
import {useAppDispatch} from "../useAppDispatch.ts";
import ModalWrapper from "./modalComponents/ModalWrapper.tsx";

interface FeedbackModalProps {
    isOpen: boolean;
    onClose: () => void;
    ticketId: string | null;
    currentUserId: string | null;
    addressedToId: string;
}

const FeedbackModal: React.FC<FeedbackModalProps> = ({ isOpen, onClose, ticketId, currentUserId, addressedToId }) => {

    const dispatch = useAppDispatch();

    const [feedbacks, setFeedbacks] = useState<FeedbackDto[]>([]);
    const [isListLoading, setIsListLoading] = useState(false);
    const [createComment, setCreateComment] = useState('');
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [editingFeedbackId, setEditingFeedbackId] = useState<string | null>(null);
    const [editCommentText, setEditCommentText] = useState('');

    useEffect(() => {
        if (!isOpen || !ticketId) return;

        const loadFeedbacks = async () => {
            setIsListLoading(true);
            setError(null);
            try {
                const result = await dispatch(fetchFeedbackByTicket(ticketId)).unwrap();
                setFeedbacks(result.feedbacks);
            } catch {
                setError("Could not load feedback history.");
            } finally {
                setIsListLoading(false);
            }
        };
        loadFeedbacks();
    }, [isOpen, ticketId, dispatch]);

    const handlePostFeedback = async () => {
        if (!currentUserId || !ticketId || !createComment.trim()) return;

        setIsSubmitting(true);
        setError(null);
        try {
            const command: CreateFeedbackDto = {
                ticketId,
                sentById: currentUserId,
                addressedToId: addressedToId,
                comment: createComment.trim(),
            };
            const newFeedback = await dispatch(addFeedbackToTicket(command)).unwrap();

            setFeedbacks(prev => [...prev, newFeedback]);
            setCreateComment('');

        } catch {
            setError("Failed to post comment.");
        } finally {
            setIsSubmitting(false);
        }
    };

    const startEdit = (feedback: FeedbackDto) => {
        setEditingFeedbackId(feedback.id);
        setEditCommentText(feedback.comment || '');
    };

    const handleEditSubmit = async (feedbackId: string) => {
        if (!editCommentText.trim()) return;

        setIsSubmitting(true);
        setError(null);
        try {
            const updatedFeedback = await dispatch(updateFeedbackAction({ id: feedbackId, newComment: editCommentText.trim() })).unwrap();

            setFeedbacks(prev => prev.map(fb =>
                fb.id === feedbackId ? { ...fb, comment: updatedFeedback.comment } : fb
            ));

            setEditingFeedbackId(null);
        } catch {
            setError("Failed to update comment.");
        } finally {
            setIsSubmitting(false);
        }
    };


    if (!isOpen || !ticketId) return null;

    return (
        <ModalWrapper isOpen={isOpen} onClose={onClose} title="Ticket Feedback" error={error}>
            <div className={styles.feedbackContainer}>
                <div className={styles.historySection}>
                    {isListLoading ? <div>Loading History...</div> : feedbacks.map(fb => (
                        <div key={fb.id} className={styles.feedbackItem}>

                            <div className={styles.feedbackHeader}>
                                <strong>{fb.sentById === currentUserId ? 'You' : 'Mentor'}</strong>
                            </div>

                            {editingFeedbackId === fb.id ? (
                                <div className={styles.editForm}>
                                    <textarea
                                        value={editCommentText}
                                        onChange={(e) => setEditCommentText(e.target.value)}
                                        rows={3}
                                        className={styles.commentInput}
                                        disabled={isSubmitting}
                                    />
                                    <div className={styles.editActions}>
                                        <button onClick={() => setEditingFeedbackId(null)} className={styles.cancelButton} disabled={isSubmitting}>Cancel</button>
                                        <button onClick={() => handleEditSubmit(fb.id)} className={styles.saveButton} disabled={isSubmitting || !editCommentText.trim()}>Save</button>
                                    </div>
                                </div>
                            ) : (<p className={styles.feedbackBody}>{fb.comment}</p>)}


                            {fb.sentById === currentUserId && editingFeedbackId !== fb.id && (
                                <button className={styles.editButton} onClick={() => startEdit(fb)}>Edit</button>
                            )}
                        </div>
                    ))}
                </div>

                <div className={styles.postSection}>
                    <textarea
                        placeholder="Leave a comment..."
                        value={createComment}
                        onChange={(e) => setCreateComment(e.target.value)}
                        rows={3}
                        className={styles.commentInput}
                        disabled={isSubmitting}
                    />
                    <button
                        onClick={handlePostFeedback}
                        disabled={isSubmitting || !createComment.trim()}
                        className={styles.postButton}
                    >
                        {isSubmitting ? 'Posting...' : 'Post Comment'}
                    </button>
                </div>
            </div>
        </ModalWrapper>
    );
};

export default FeedbackModal;
