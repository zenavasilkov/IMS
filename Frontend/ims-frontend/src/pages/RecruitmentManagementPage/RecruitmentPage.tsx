import React, {useCallback, useEffect, useState} from 'react';
import { useSelector } from 'react-redux';
import { useAuth0 } from '@auth0/auth0-react';
import type { RootState } from '../../store';
import { useAppDispatch } from '../../components/useAppDispatch';
import PageLoader from '../../components/common/pageLoader/PageLoader';
import styles from './RecruitmentPage.module.css';
import {RecruitmentIcon} from "../../components/common/Icons.tsx";
import {
    acceptCandidate,
    fetchCandidateByEmail,
    fetchCandidates, resetRecruitmentFilters, setCandidatePage, setFilterEmail
} from "../../features/slices/recruitmentSlice.ts";
import type {FindCandidateByIdQueryResponse} from "../../entities/recruitment/dto/candidate_dto.ts";
import ScheduleInterviewModal from "../../components/modals/ScheduleInterviewModal.tsx";
import RegisterCandidateModal from "../../components/modals/RegisterCandidateModal.tsx";
import commonStyles from '../../components/common/commonStyles/commonPageStyles.module.css'
import UpdateCvModal from "../../components/modals/UpdateCvModal.tsx";
import PaginationControls from "../../components/PaginationControls.tsx";
import PageLayout from "../../components/PageLayout.tsx";
import SimpleListHeader from "../../components/SimpleListHeader.tsx";
import useMinLoadingTime from "../../hooks/useMinLoadingTime.ts";
import {candidateService} from "../../api/services/recruitment";

const RECRUITMENT_HEADER_CONFIG = [
    { label: 'Name/Contact', flex: 2, textAlign: 'left' as const },
    { label: 'Status', flex: 0.5, textAlign: 'center' as const },
    { label: 'Actions', flex: 2, textAlign: 'center' as const },
];

const RecruitmentPage: React.FC = () => {
    const { isAuthenticated, isLoading: isAuth0Loading } = useAuth0();
    const dispatch = useAppDispatch();
    const { candidates, loading, error, page, totalPages, pageSize, acceptingCandidates, filterEmail } = useSelector((state: RootState) => state.recruitment);
    const [isInterviewModalOpen, setIsInterviewModalOpen] = useState(false);
    const [candidateToSchedule, setCandidateToSchedule] = useState<FindCandidateByIdQueryResponse | undefined>(undefined);
    const [isRegisterModalOpen, setIsRegisterModalOpen] = useState(false);
    const [isCvModalOpen, setIsCvModalOpen] = useState(false);
    const [candidateToUpdateCv, setCandidateToUpdateCv] = useState<FindCandidateByIdQueryResponse | undefined>(undefined);
    const showPageLoader = useMinLoadingTime(loading);

    const fetchCandidatesData = useCallback(() => {
        if (!isAuth0Loading && isAuthenticated) {
            const params = {
                pageNumber: page,
                pageSize: pageSize
            };
            dispatch(fetchCandidates(params));
        }
    }, [page, isAuthenticated, isAuth0Loading, dispatch, pageSize]);

    const handleViewCv = async (candidateId: string) => {
        try {
            const url = await candidateService.getCvViewUrl(candidateId);
            if (url) {
                window.open(url, '_blank');
            } else {
                alert("Could not retrieve file URL");
            }
        } catch (error) {
            console.error("Failed to open CV", error);
            alert("Failed to open CV. It might not exist.");
        }
    };

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

    if (showPageLoader) return <PageLoader loadingText="Loading candidates..." />;
    if (error) return <div className={commonStyles.errorMessage}>{error}</div>;

    return(
        <PageLayout
            title="Recruitment Management"
            Icon={RecruitmentIcon}
            iconColor="#007bff"
            createButton={<button onClick={handleRegisterCandidate} className={commonStyles.createButton}>Register Candidate </button>}
        >
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
                <SimpleListHeader columns={RECRUITMENT_HEADER_CONFIG} />
                {candidates.map(candidate => {
                    const isBeingAccepted = acceptingCandidates.includes(candidate.id);
                    const isDisabled = candidate.isApplied || isBeingAccepted;

                    return (
                        <div key={candidate.id} className={styles.candidateItem}>
                            <div className={styles.candidateInfo}>
                                <span className={styles.name}>{candidate.firstName} {candidate.lastName}</span>
                                <span className={styles.contact}>{candidate.email} | {candidate.phoneNumber}</span>
                            </div>
                            <span className={styles.status} style={{ flex: 0.5, textAlign: 'center' }}>{candidate.isApplied ? 'Accepted' : 'Invited'}</span>
                            <div className={styles.actions} style={{ flex: 2, textAlign: 'center' }}>
                                <button className={commonStyles.actionButton} disabled={isDisabled} onClick={() => handleScheduleInterview(candidate.id)}>Interview</button>
                                <button className={commonStyles.actionButton} disabled={isDisabled} onClick={() => handleAccept(candidate.id)}>Accept</button>
                                <button className={commonStyles.actionButton} onClick={() => handleUpdateCv(candidate)}>Update CV</button>
                                {candidate.cvLink && (
                                    <button
                                        className={commonStyles.actionButton}
                                        onClick={() => handleViewCv(candidate.id)}
                                        title="View CV"
                                    >
                                        View CV
                                    </button>
                                )}
                            </div>
                        </div>
                    );
                })}
            </div>

            <PaginationControls
                currentPage={page}
                totalPages={totalPages}
                onPreviousPage={() => dispatch(setCandidatePage(page - 1))}
                onNextPage={() => dispatch(setCandidatePage(page + 1))}
                hasContent={candidates.length > 0}
            />

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
            />
        </PageLayout>
    );
};

export default RecruitmentPage;
