import React, { useState, useEffect } from 'react';
import styles from '../common/commonStyles/commonModalStyles.module.css'
import type {CreateUserDto, UpdateUserDto, UserDto} from "../../entities/ims/dto/user_dto.ts";
import {userService} from "../../api/services";
import {Role} from "../../entities/ims/enums.ts";

interface UserFormModalProps {
    isOpen: boolean;
    onClose: () => void;
    onSuccess: () => void;
    initialUser?: UserDto;
}

const EMPTY_FORM_DATA: CreateUserDto = {
    email: '',
    phoneNumber: '',
    firstname: '',
    lastname: '',
    patronymic: '',
    role: Role.Intern,
};

const PHONE_NUMBER_REGEX = String.raw`^\+?[1-9]\d{1,14}$`;

const UserFormModal: React.FC<UserFormModalProps> = ({ isOpen, onClose, onSuccess, initialUser }) => {

    const isEditMode = !!initialUser;

    const [formData, setFormData] = useState<CreateUserDto | UpdateUserDto>({
        email: initialUser?.email || '',
        phoneNumber: initialUser?.phoneNumber || '',
        firstname: initialUser?.firstname || '',
        lastname: initialUser?.lastname || '',
        patronymic: initialUser?.patronymic || '',
        role: initialUser?.role || Role.Intern,
    });
    const [isSubmitting, setIsSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (isEditMode && initialUser) {
            setFormData({
                email: initialUser.email || '',
                phoneNumber: initialUser.phoneNumber || '',
                firstname: initialUser.firstname || '',
                lastname: initialUser.lastname || '',
                patronymic: initialUser.patronymic || '',
                role: initialUser.role,
            });
        } else {
            setFormData(EMPTY_FORM_DATA);
        }

        setError(null);
    }, [initialUser, isEditMode, isOpen]);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        let newValue = value;

        if (name === 'phoneNumber') {
            newValue = value.replaceAll(/[^0-9+]/g, '');
        }
        
        setFormData(prev => ({
            ...prev,
            [name]: name === 'role' ? Number(newValue) : newValue
        }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setIsSubmitting(true);
        setError(null);

        try {
            if (isEditMode && initialUser) {
                await userService.updateUser(initialUser.id, formData as UpdateUserDto);
            } else {
                const dataToCreate: CreateUserDto = {
                    ...(formData as CreateUserDto),
                    role: formData.role!,
                };
                await userService.createUser(dataToCreate);
            }

            onSuccess();
            onClose();
        } catch (err: any) {
            console.error("User submission failed:", err);
            setError(err.response?.data?.message || `Failed to ${isEditMode ? 'update' : 'create'} user.`);
        } finally {
            setIsSubmitting(false);
        }
    };

    if (!isOpen) return null;

    const roleOptions = Object.keys(Role).filter(key => Number.isNaN(Number(key))).map(key => ({
        value: String(Role[key as keyof typeof Role]),
        label: key.replaceAll(/([A-Z])/g, ' $1').trim()
    }))

    let buttonText = 'Create Account';
    if (isEditMode) {
        buttonText = 'Save Changes';
    }
    if (isSubmitting) {
        buttonText = 'Saving...';
    }

    return (
        <div className={styles.modalOverlay}>
            <div className={styles.modalContent}>
                <h2 className={styles.modalTitle}>
                    {isEditMode ? 'Edit User' : 'Create New User'}
                </h2>
                <button className={styles.closeButton} onClick={onClose}>&times;</button>

                <form onSubmit={handleSubmit} className={styles.form}>
                    {error && <div className={styles.error}>{error}</div>}

                    <label>
                        First Name<input name="firstname" value={formData.firstname || ''} onChange={handleChange} required />
                    </label>
                    
                    <label>
                        Last Name<input name="lastname" value={formData.lastname || ''} onChange={handleChange} required />
                    </label>

                    <label>
                        Email<input
                            name="email"
                            type="email"
                            value={formData.email || ''}
                            onChange={handleChange}
                            required
                            disabled={isEditMode}
                        />
                    </label>                    

                    <label>
                        Phone Number<input
                            name="phoneNumber"
                            type="tel"
                            value={formData.phoneNumber || ''}
                            onChange={handleChange}
                            pattern={PHONE_NUMBER_REGEX}
                            title="Phone number must start with an optional '+' and be 2-15 digits (E.164 format)."/>
                    </label>                    

                    <label>
                        Role<select name="role" value={String(formData.role)} onChange={handleChange} required>
                            {roleOptions.map(option => (
                                <option key={option.value} value={option.value}>{option.label}</option>
                            ))}
                        </select>
                    </label>

                    <button type="submit" disabled={isSubmitting} className={styles.submitButton}>
                        {buttonText}
                    </button>
                </form>
            </div>
        </div>
    );
};

export default UserFormModal;
