import axios from 'axios';

const API_BASE = import.meta.env?.VITE_API_BASE_URL ?? 'http://localhost:8090/api/';
const RECRUITMENT_BASE = import.meta.env?.VITE_RECRUITMENT_API_BASE_URL ?? 'http://localhost:8110/api/';

export const ImsApi = axios.create({
    baseURL: API_BASE,
    headers: {
        'Content-Type': 'application/json',
    }
});

export const RecruitmentApi = axios.create({
    baseURL: RECRUITMENT_BASE,
    headers: {
        'Content-Type': 'application/json',
    }
});
