import React, {useCallback, useEffect, useState} from 'react';
import { useSelector } from 'react-redux';
import { useAuth0 } from '@auth0/auth0-react';
import type { RootState } from '../../store';
import { useAppDispatch } from '../../components/useAppDispatch';
import PageLoader from '../../components/common/pageLoader/PageLoader';
import styles from './RecruitmentPage.module.css';
import { RecruitmentIcon } from "../../components/common/Icons.tsx";
import {
    acceptCandidate,
    fetchCandidateByEmail,
    fetchCandidates, resetRecruitmentFilters,
    setCandidatePage, setFilterEmail
} from "../../features/slices/recruitmentSlice.ts";
import type {FindCandidateByIdQueryResponse} from "../../entities/recruitment/dto/candidate_dto.ts";
import ScheduleInterviewModal from "../../components/modals/ScheduleInterviewModal.tsx";
import RegisterCandidateModal from "../../components/modals/RegisterCandidateModal.tsx";
import commonStyles from '../../components/common/commonStyles/commonPageStyles.module.css'
import UpdateCvModal from "../../components/modals/UpdateCvModal.tsx";

const RecruitmentPage: React.FC = () => {
    const { isAuthenticated, isLoading: isAuth0Loading } = useAuth0();
    const dispatch = useAppDispatch();
    const { candidates, loading, error, page, totalPages, pageSize, acceptingCandidates, filterEmail } = useSelector((state: RootState) => state.recruitment);
    const [isInterviewModalOpen, setIsInterviewModalOpen] = useState(false);
    const [candidateToSchedule, setCandidateToSchedule] = useState<FindCandidateByIdQueryResponse | undefined>(undefined);
    const [isRegisterModalOpen, setIsRegisterModalOpen] = useState(false);
    const [isCvModalOpen, setIsCvModalOpen] = useState(false);
    const [candidateToUpdateCv, setCandidateToUpdateCv] = useState<FindCandidateByIdQueryResponse | undefined>(undefined);

    const fetchCandidatesData = useCallback(() => {
        if (!isAuth0Loading && isAuthenticated) {
            const params = {
                pageNumber: page,
                pageSize: pageSize
            };
            dispatch(fetchCandidates(params));
        }
    }, [page, isAuthenticated, isAuth0Loading, dispatch, pageSize]);

    useEffect(() => {
        fetchCandidatesData()
    }, [fetchCandidatesData, acceptingCandidates ]);

    const handleInterviewSuccess = useCallback(() => {
        setIsInterviewModalOpen(false);
        setCandidateToSchedule(undefined);
        fetchCandidatesData();
    }, [fetchCandidatesData]);

    const handleAccept = (candidateId: string) => {
        if (acceptingCandidates.includes(candidateId)) return;
        dispatch(acceptCandidate(candidateId));
    };
    const handleRegisterCandidate = () => setIsRegisterModalOpen(true);

    const handleScheduleInterview = (candidateId: string) => {
        const candidate = candidates.find(c => c.id === candidateId);
        if (candidate) {
            setCandidateToSchedule(candidate);
            setIsInterviewModalOpen(true);
        }
    };

    const handleSearchClick = () => {
        if (filterEmail) {
            dispatch(fetchCandidateByEmail(filterEmail));
        } else {
            dispatch(resetRecruitmentFilters());
            fetchCandidatesData();
        }
    };

    const handleResetClick = () => {
        dispatch(resetRecruitmentFilters());
        fetchCandidatesData();
    };

    const handleRegistrationSuccess = () => {
        setIsRegisterModalOpen(false);
        fetchCandidatesData();
    }

    const handleUpdateCv = (candidate: FindCandidateByIdQueryResponse) => {
        setCandidateToUpdateCv(candidate);
        setIsCvModalOpen(true);
    };

    const handleCvUpdateSuccess = handleRegistrationSuccess;

    if (loading) return <PageLoader loadingText="Loading candidates..." />;
    if (error) return <div className={commonStyles.errorMessage}>{error}</div>;

    return (
        <div className={commonStyles.container}>
            <div className={styles.titleArea}>
                <h1 className={commonStyles.heading}>
                    <RecruitmentIcon className={styles.headingIcon} />
                    Recruitment Portal
                </h1>

                <button
                    onClick={handleRegisterCandidate}
                    className={commonStyles.createButton}
                >
                    Register Candidate
                </button>
            </div>

            <div className={styles.filterBar}>
                <input
                    type="text"
                    placeholder="Search Candidates by Email..."
                    className={styles.searchInput} value={filterEmail}
                    onChange={(e) => dispatch(setFilterEmail(e.target.value))} />
                <button className={commonStyles.actionButton} onClick={handleSearchClick}>Search</button>
                <button className={commonStyles.actionButton} onClick={handleResetClick}>Reset</button>
            </div>

            <div className={styles.candidateList}>
                <div className={commonStyles.listHeader}>
                    <span style={{ flex: 2, textAlign: 'left' }}>Name / Contact</span>
                    <span style={{ flex: 1 }}>Status</span>
                    <span style={{ flex: 1.5 }}>Actions</span>
                </div>
                {candidates.map(candidate => {
                    const isBeingAccepted = acceptingCandidates.includes(candidate.id);
                    const isDisabled = candidate.isApplied || isBeingAccepted;

                    return (
                        <div key={candidate.id} className={styles.candidateItem}>
                            <div className={styles.candidateInfo}>
                                <span className={styles.name}>{candidate.firstName} {candidate.lastName}</span>
                                <span className={styles.contact}>{candidate.email} | {candidate.phoneNumber}</span>
                            </div>
                            <span className={styles.status}>{candidate.isApplied ? 'Accepted' : 'Invited'}</span>
                            <div className={styles.actions}>
                                <button className={commonStyles.actionButton} disabled={isDisabled} onClick={() => handleScheduleInterview(candidate.id)}>Interview</button>
                                <button className={commonStyles.actionButton} disabled={isDisabled} onClick={() => handleAccept(candidate.id)}>Accept</button>
                                <button className={commonStyles.actionButton} onClick={() => handleUpdateCv(candidate)}>Update CV</button>
                            </div>
                        </div>
                    );
                })}
            </div>

            <div className={commonStyles.pagination}>
                <button disabled={page === 1} onClick={() => dispatch(setCandidatePage(page - 1))}>Previous</button>
                <span>Page {page} of {totalPages}</span>
                <button disabled={page >= totalPages} onClick={() => dispatch(setCandidatePage(page + 1))}>Next</button>
            </div>

            <ScheduleInterviewModal
                isOpen={isInterviewModalOpen}
                onClose={() => { setIsInterviewModalOpen(false); setCandidateToSchedule(undefined); }}
                onSuccess={handleInterviewSuccess}
                candidate={candidateToSchedule}
            />

            <RegisterCandidateModal
                isOpen={isRegisterModalOpen}
                onClose={() => setIsRegisterModalOpen(false)}
                onSuccess={handleRegistrationSuccess}
            />

            <UpdateCvModal
                isOpen={isCvModalOpen}
                onClose={() => setIsCvModalOpen(false)}
                onSuccess={handleCvUpdateSuccess}
                candidateId={candidateToUpdateCv?.id || ''}
                currentCvLink={candidateToUpdateCv?.cvLink}
            />
        </div>
    );
};

export default RecruitmentPage;
