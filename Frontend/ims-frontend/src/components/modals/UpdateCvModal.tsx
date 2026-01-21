import React, { useState, useEffect } from 'react';
import styles from '../common/commonStyles/commonModalStyles.module.css'
import {useAppDispatch} from "../useAppDispatch.ts";
import ModalWrapper from "./modalComponents/ModalWrapper.tsx";
import {uploadCandidateCv} from "../../features/slices/recruitmentSlice.ts";

interface UpdateCvModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSuccess: () => void;
    candidateId: string;
}

const UpdateCvModal: React.FC<UpdateCvModalProps> = ({ isOpen, onClose, onSuccess, candidateId }) => {
    const dispatch = useAppDispatch();
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [isDragging, setIsDragging] = useState(false);

    useEffect(() => {
        if (isOpen) {
            setSelectedFile(null);
            setError(null);
            setIsDragging(false);
        }
    }, [isOpen]);

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files.length > 0) {
            validateAndSetFile(e.target.files[0]);
        }
    };

    const validateAndSetFile = (file: File) => {
        if (file.type !== 'application/pdf') {
            setError('Only PDF files are allowed.');
            return;
        }
        if (file.size > 10 * 1024 * 1024) {
            setError('File size must be less than 10MB.');
            return;
        }
        setError(null);
        setSelectedFile(file);
    };

    const onDragOver = (e: React.DragEvent<HTMLDivElement>) => {
        e.preventDefault();
        setIsDragging(true);
    };

    const onDragLeave = (e: React.DragEvent<HTMLDivElement>) => {
        e.preventDefault();
        setIsDragging(false);
    };

    const onDrop = (e: React.DragEvent<HTMLDivElement>) => {
        e.preventDefault();
        setIsDragging(false);
        if (e.dataTransfer.files && e.dataTransfer.files.length > 0) {
            validateAndSetFile(e.dataTransfer.files[0]);
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!selectedFile) {
            setError("Please select a file.");
            return;
        }

        setIsSubmitting(true);
        setError(null);

        try {
            await dispatch(uploadCandidateCv({ id: candidateId, file: selectedFile })).unwrap();
            onSuccess();
            onClose();
        } catch (err: any) {
            const message = err.message || 'Failed to upload CV.';
            setError(message);
            console.error(message);
        } finally {
            setIsSubmitting(false);
        }
    };

    if (!isOpen) return null;

    return (
        <ModalWrapper isOpen={isOpen} onClose={onClose} title="Upload Candidate CV" error={error}>
            <form onSubmit={handleSubmit} className={styles.form}>

                <div
                    className={`${styles.dropZone} ${isDragging ? styles.dragging : ''}`}
                    onDragOver={onDragOver}
                    onDragLeave={onDragLeave}
                    onDrop={onDrop}
                    onClick={() => document.getElementById('fileInput')?.click()}
                >
                    <input
                        type="file"
                        id="fileInput"
                        accept="application/pdf"
                        onChange={handleFileChange}
                        hidden
                    />

                    {selectedFile ? (
                        <div className={styles.fileInfo}>
                            <span className={styles.icon}>📄</span>
                            <span className={styles.fileName}>{selectedFile.name}</span>
                            <span className={styles.fileSize}>({(selectedFile.size / 1024).toFixed(1)} KB)</span>
                        </div>
                    ) : (
                        <div className={styles.placeholder}>
                            <span className={styles.uploadIcon}>☁️</span>
                            <p>Drag & Drop PDF here</p>
                            <span className={styles.orText}>or click to browse</span>
                        </div>
                    )}
                </div>

                <button
                    type="submit"
                    disabled={isSubmitting || !selectedFile}
                    className={styles.submitButton}
                >
                    {isSubmitting ? 'Uploading...' : 'Upload CV'}
                </button>
            </form>
        </ModalWrapper>
    );
};

export default UpdateCvModal;
