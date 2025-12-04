import { configureStore } from '@reduxjs/toolkit';
import userManagementReducer from './features/userManagement/userManagementSlice';
import authReducer from './features/auth/authSlice';

export const store = configureStore({
    reducer: {
        userManagement: userManagementReducer,
        auth: authReducer,
    },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;