import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import type { User } from '@auth0/auth0-react';

interface AuthState {
  isAuthenticated: boolean;
  user: User | null;
  isLoading: boolean;
}

const initialState: AuthState = {
  isAuthenticated: false,
  user: null,
  isLoading: true, 
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setAuthStatus: (state, action: PayloadAction<{ isAuthenticated: boolean; user: User | null; isLoading: boolean }>) => {
      state.isAuthenticated = action.payload.isAuthenticated;
      state.user = action.payload.user;
      state.isLoading = action.payload.isLoading;
    },
  },
});

export const { setAuthStatus } = authSlice.actions;

export default authSlice.reducer;

