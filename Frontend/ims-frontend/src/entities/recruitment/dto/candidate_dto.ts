export interface RegisterCandidateCommand {
  firstName?: string | null;
  lastName?: string | null;
  email?: string | null;
  phoneNumber?: string | null;
  cvLink?: string | null;
  linkedIn?: string | null;
  patronymic?: string | null;
}

export interface FindCandidateByEmailQueryResponse {
  id: string;
  firstName?: string | null;
  lastName?: string | null;
  email?: string | null;
  isApplied: boolean;
  phoneNumber?: string | null;
  cvLink?: string | null;
  linkedIn?: string | null;
  patronymic?: string | null;
}

export interface FindCandidateByIdQueryResponse {
  id: string;
  firstName?: string | null;
  lastName?: string | null;
  email?: string | null;
  isApplied: boolean;
  phoneNumber?: string | null;
  cvLink?: string | null;
  linkedIn?: string | null;
  patronymic?: string | null;
}

export interface FindCandidateByIdQueryResponsePagedList {
  items: FindCandidateByIdQueryResponse[] | null;
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface GetAllCandidatesQueryResponse {
  candidates: FindCandidateByIdQueryResponsePagedList;
}

export interface UpdateCvLinkCommand {
  id: string;
  newCvLink?: string | null;
}
