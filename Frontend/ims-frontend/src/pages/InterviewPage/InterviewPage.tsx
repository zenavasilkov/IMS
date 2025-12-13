import React, { useEffect, useState, useCallback } from 'react';
import { useSelector } from 'react-redux';
import { useAuth0 } from '@auth0/auth0-react';
import type { RootState } from '../../store';
import { useAppDispatch } from '../../components/useAppDispatch';
import PageLoader from '../../components/common/pageLoader/PageLoader';
import {
    fetchInterviews, setInterviewPage, setFilterCandidateEmail, resetInterviewFilters, setFilterCandidateId, passInterview
} from '../../features/slices/interviewSlice';
import styles from './InterviewPage.module.css';
import commonStyles from '../../components/common/commonStyles/commonPageStyles.module.css';
import { InterviewType, EmployeeRole } from '../../entities/recruitment/enums';
import RescheduleInterviewModal from "../../components/modals/RescheduleInterviewModal.tsx";
import {interviewService, candidateService, employeeService} from "../../api/services/recruitment";
import {InterviewIcon} from "../../components/common/Icons.tsx";
import type { FindCandidateByIdQueryResponse } from "../../entities/recruitment/dto/candidate_dto";
import type { GetEmployeeByIdQueryResponse } from "../../entities/recruitment/dto/employee_dto";
import {fetchCandidateByEmail} from "../../features/slices/recruitmentSlice.ts";
import PaginationControls from "../../components/PaginationControls.tsx";
import PageLayout from "../../components/PageLayout.tsx";
import SimpleListHeader from "../../components/SimpleListHeader.tsx";

const getStatusClass = (interview: any) => {
    if (interview.isCancelled) return styles.statusCancelled;
    if (interview.isPassed) return styles.statusPassed;
    return styles.statusPending;
};

const INTERVIEW_HEADER_CONFIG = [
    { label: 'Candidate', flex: 3, textAlign: 'center' as const },
    { label: 'Interviewer', flex: 3.2, textAlign: 'center' as const },
    { label: 'Type/Date', flex: 2, textAlign: 'center' as const },
    { label: 'Status', flex: 1, textAlign: 'center' as const },
    { label: 'Actions', flex: 3, textAlign: 'center' as const },
];

const InterviewPage: React.FC = () => {
    const { isAuthenticated, isLoading: isAuth0Loading } = useAuth0();
    const dispatch = useAppDispatch();
    const { interviews, loading, error, page, totalPages, pageSize, filterCandidateEmail, filterCandidateId } = useSelector((state: RootState) => state.interview);
    const [isRescheduleModalOpen, setIsRescheduleModalOpen] = useState(false);
    const [interviewToReschedule, setInterviewToReschedule] = useState<any>(undefined);
    const [candidatesMap, setCandidatesMap] = useState<Map<string, FindCandidateByIdQueryResponse>>(new Map());
    const [employeesMap, setEmployeesMap] = useState<Map<string, GetEmployeeByIdQueryResponse>>(new Map());

    const fetchInterviewsData = useCallback(async () => {
        if (!isAuth0Loading && isAuthenticated) {
            await dispatch(fetchInterviews({ pageNumber: page, pageSize }));
        }
    }, [page, isAuthenticated, isAuth0Loading, dispatch, pageSize, filterCandidateId]);

    const handlePassInterview = (interviewId: string) => {
        if (globalThis.confirm("Mark this interview as PASSED?")) {
            dispatch(passInterview(interviewId));
        }
    };

    useEffect(() => {
        fetchInterviewsData();
    }, [fetchInterviewsData]);

    const fetchRelatedEntities = useCallback(async () => {
        if (interviews.length <= 0 || !isAuthenticated) return;

        const newCandidatesMap = new Map<string, FindCandidateByIdQueryResponse>(candidatesMap);
        const newEmployeesMap = new Map<string, GetEmployeeByIdQueryResponse>(employeesMap);

        for (const interview of interviews) {
            if (interview.candidateId && !newCandidatesMap.has(interview.candidateId)) {
                try {
                    const candidate = await candidateService.getCandidateById(interview.candidateId);
                    newCandidatesMap.set(interview.candidateId, candidate);
                } catch (e) {
                    console.error(`Failed to fetch candidate ${interview.candidateId}:`, e);
                }
            }

            if (interview.interviewerId && !newEmployeesMap.has(interview.interviewerId)) {
                try {
                    const employee = await employeeService.getEmployeeById(interview.interviewerId);
                    newEmployeesMap.set(interview.interviewerId, employee);
                } catch (e) {
                    console.error(`Failed to fetch employee ${interview.interviewerId}:`, e);
                }
            }
        }
        setCandidatesMap(newCandidatesMap);
        setEmployeesMap(newEmployeesMap);
    }, [interviews, isAuthenticated]);

    useEffect(() => {
        fetchRelatedEntities();
    }, [fetchRelatedEntities]);


    const handleEmailSearch = () => {
        if (filterCandidateEmail) {
            dispatch(fetchCandidateByEmail(filterCandidateEmail));
        } else {
            dispatch(setFilterCandidateId(undefined));
        }
    };

    const handleReset = () => dispatch(resetInterviewFilters());

    const handleReschedule = (interview: any) => {
        setInterviewToReschedule(interview);
        setIsRescheduleModalOpen(true);
    };

    const handleCancel = async (interviewId: string) => {
        if (globalThis.confirm("Are you sure you want to cancel this interview?")) {
            try {
                await interviewService.cancelInterview(interviewId);
                fetchInterviewsData();
            } catch (e) {
                console.error("Failed to cancel interview:", e);
            }
        }
    };

    const handleSuccess = () => {
        setIsRescheduleModalOpen(false);
        setInterviewToReschedule(undefined);
        fetchInterviewsData();
    };

    const getInterviewTypeDisplay = (type: InterviewType) => {
        return InterviewType[type].replaceAll(/([A-Z])/g, ' $1').trim();
    };

    const getRoleDisplayName = useCallback((role: number) => {
        const roleKeyName = EmployeeRole[role];
        return roleKeyName ? roleKeyName.replaceAll(/([A-Z])/g, ' $1').trim() : 'N/A';
    }, []);


    if (loading) return <PageLoader loadingText="Loading interviews..." />;
    if (error) return <div className={commonStyles.errorMessage}>{error}</div>;

    return(
        <PageLayout
            title="Interview Management"
            Icon={InterviewIcon}
            iconColor="#ff3131"
            createButton={null}
        >
            <div className={commonStyles.filterBar}>
                <input
                    type="text"
                    className={commonStyles.searchInput}
                    placeholder="Search by Candidate Email"
                    value={filterCandidateEmail}
                    onChange={(e) => dispatch(setFilterCandidateEmail(e.target.value))}
                />
                <button className={commonStyles.actionButton} onClick={handleEmailSearch}>Search</button>
                <button className={commonStyles.actionButton} onClick={handleReset}>Reset</button>
            </div>

            <div className={styles.interviewList}>
                <SimpleListHeader columns={INTERVIEW_HEADER_CONFIG} />
                {interviews.map(interview => {
                    const candidate = candidatesMap.get(interview.candidateId || '');
                    const employee = employeesMap.get(interview.interviewerId || '');

                    return (
                        <div key={interview.id} className={commonStyles.userItem}>
                            <div className={styles.candidateInfo} style={{ flex: 3 }}>
                                <span className={styles.name}>{candidate?.firstName} {candidate?.lastName || interview.candidateEmail?.split('@')[0]}</span><br/>
                                <span className={styles.contact}>{candidate?.email || interview.candidateEmail} <br/> {candidate?.phoneNumber || 'N/A'}</span>
                            </div>

                            <div className={styles.interviewerInfo} style={{ flex: 3.2 }}>
                                <span className={styles.name}>{employee?.firstName} {employee?.lastName || interview.interviewerEmail?.split('@')[0]}</span>	&ensp;
                                <span className={styles.contact}>{employee?.email || interview.interviewerEmail} <span className={styles.userRoleText}>({employee?.role ? getRoleDisplayName(employee.role) : 'N/A'})</span></span>	&ensp;
                                <span className={styles.departmentName}>{interview.deparnmentName || 'N/A'}</span>
                            </div>

                            <span style={{ flex: 2 }} className={styles.interviewDetailText}>
                                {getInterviewTypeDisplay(interview.interviewType)} - {new Date(interview.scheduledAt).toLocaleDateString()}
                            </span>

                            <span style={{ flex: 1 }} className={getStatusClass(interview)}>
                                {interview.isCancelled ? 'Cancelled' : (interview.isPassed ? 'Passed' : 'Pending')}
                            </span>

                            <div className={commonStyles.actions} style={{ flex: 3 }}>
                                <button className={commonStyles.actionButton} disabled={interview.isCancelled || interview.isPassed} onClick={() => handlePassInterview(interview.id)}>Pass</button>
                                <button className={commonStyles.actionButton} disabled={interview.isCancelled || interview.isPassed} onClick={() => handleReschedule(interview)}>Reschedule</button>
                                <button className={commonStyles.actionButton} disabled={interview.isCancelled || interview.isPassed} onClick={() => handleCancel(interview.id)}>Cancel</button>
                            </div>
                        </div>
                    );
                })}
            </div>

            <PaginationControls
                currentPage={page}
                totalPages={totalPages}
                onPreviousPage={() => dispatch(setInterviewPage(page - 1))}
                onNextPage={() => dispatch(setInterviewPage(page + 1))}
                hasContent={interviews.length > 0}
            />

            <RescheduleInterviewModal
                isOpen={isRescheduleModalOpen}
                onClose={() => setIsRescheduleModalOpen(false)}
                onSuccess={handleSuccess}
                interview={interviewToReschedule}
            />
        </PageLayout>
    );
};

export default InterviewPage;
