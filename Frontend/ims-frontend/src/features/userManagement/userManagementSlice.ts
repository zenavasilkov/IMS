import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { userService } from '../../api/services/userService';
import type { UserDto } from '../../entities/ims/dto/user_dto';

interface UserManagementState {
  users: UserDto[];
  loading: boolean;
  error: string | null;
  page: number;
  totalPages: number;
  pageSize: number;
}

const initialState: UserManagementState = {
  users: [],
  loading: false,
  error: null,
  page: 1,
  totalPages: 1,
  pageSize: 10,
};

export const fetchUsers = createAsyncThunk(
  'userManagement/fetchUsers',
  async ({ pageNumber, pageSize }: { pageNumber: number; pageSize: number }, { rejectWithValue }) => {
    try {
      const result = await userService.getAllUsers(pageNumber, pageSize);
      return { items: result.items, totalCount: result.totalCount };
    } catch (err: any) {
      const errorMessage = err.response?.data?.message || err.message || "An unknown error occurred.";
      return rejectWithValue(`Failed to load user data: ${errorMessage}. Please check API server status and your network connection. If the issue persists, your user role might not have 'read:users' permission on the backend.`);
    }
  }
);

const userManagementSlice = createSlice({
  name: 'userManagement',
  initialState,
  reducers: {
    setPage: (state, action: { payload: number }) => {
      state.page = action.payload;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchUsers.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchUsers.fulfilled, (state, action) => {
        state.loading = false;
        state.users = action.payload.items || [];
        state.totalPages = Math.ceil(action.payload.totalCount / state.pageSize);
      })
      .addCase(fetchUsers.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload as string;
        state.users = [];
      });
  },
});

export const { setPage } = userManagementSlice.actions;

export default userManagementSlice.reducer;
