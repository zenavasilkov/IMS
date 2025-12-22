import { createSlice, type PayloadAction } from '@reduxjs/toolkit';
import { PORTALS } from '../../components/portalSwitcher/PortalSwitcher';

interface AppState {
    activePortal: string;
}

const initialState: AppState = {
    activePortal: PORTALS.USER_MANAGEMENT, //TODO change to unauthorized portal
};

const appSlice = createSlice({
    name: 'app',
    initialState,
    reducers: {
        setAppActivePortal: (state, action: PayloadAction<string>) => {
            state.activePortal = action.payload;
        },
    },
});

export const { setAppActivePortal } = appSlice.actions;
export default appSlice.reducer;
