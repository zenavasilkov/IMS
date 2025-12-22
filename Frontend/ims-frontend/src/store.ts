import { configureStore } from '@reduxjs/toolkit';
import userManagementReducer from './features/slices/userManagementSlice.ts';
import authReducer from './features/slices/authSlice.ts';
import recruitmentReducer from './features/slices/recruitmentSlice.ts'
import employeeReducer from './features/slices/employeeSlice.ts';
import departmentReducer from './features/slices/departmentSlice.ts';
import interviewReducer from './features/slices/interviewSlice.ts';
import internshipReducer from './features/slices/internshipSlice';
import mentorInternsReducer from './features/slices/mentorInternsSlice';
import boardKanbanReducer from './features/slices/boardKanbanSlice';
import appReducer from "./features/slices/appSlice";

export const store = configureStore({
    reducer: {
        userManagement: userManagementReducer,
        auth: authReducer,
        recruitment: recruitmentReducer,
        employee: employeeReducer,
        department: departmentReducer,
        interview: interviewReducer,
        internship: internshipReducer,
        mentorInterns: mentorInternsReducer,
        boardKanban: boardKanbanReducer,
        app: appReducer,
    },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
