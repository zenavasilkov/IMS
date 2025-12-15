import {createSlice, createAsyncThunk, type PayloadAction} from '@reduxjs/toolkit';
import { userService } from '../../api/services';
import type { UserDto } from '../../entities/ims/dto/user_dto.ts';
import type { Role } from '../../entities/ims/enums.ts';
import { UserSortingParameter } from '../../entities/ims/enums.ts';
import type {FetchUsersParams} from "../../entities/ims/FetchParameters.ts";

interface UserManagementState {
  users: UserDto[];
  loading: boolean;
  error: string | null;
  page: number;
  totalPages: number;
  pageSize: number;

  filterFirstName: string;
  filterLastName: string;
  filterRole: Role | '';
  sortParameter: UserSortingParameter;
}

const initialState: UserManagementState = {
  users: [],
  loading: false,
  error: null,
  page: 1,
  totalPages: 1,
  pageSize: 10,

  filterFirstName: '',
  filterLastName: '',
  filterRole: '',
  sortParameter: UserSortingParameter.None,
};

export const fetchUsers = createAsyncThunk(
  'userManagement/fetchUsers',
  async (params : FetchUsersParams , { rejectWithValue }) => {
    try {
      const result = await userService.getAllUsers(params);
      return { items: result.items, totalCount: result.totalCount || 1 };
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
    setPage: (state, action: PayloadAction<number>) => {
      state.page = action.payload;
    },
    setFilterFirstName : (state, action : PayloadAction<string>) => {
      state.filterFirstName = action.payload;
      state.page = 1;
    },
    setFilterLastName: (state, action: PayloadAction<string>) => {
      state.filterLastName = action.payload;
      state.page = 1;
    },
    setFilterRole: (state, action: PayloadAction<Role | ''>) => {
      state.filterRole = action.payload;
      state.page = 1;
    },
    setSortParameter: (state, action: PayloadAction<UserSortingParameter>) => {
      state.sortParameter = action.payload;
      state.page = 1;
    },
    resetFilters: (state) => {
      state.filterFirstName = initialState.filterFirstName;
      state.filterLastName = initialState.filterLastName;
      state.filterRole = initialState.filterRole;
      state.sortParameter = initialState.sortParameter;
      state.page = 1;
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

export const {
  setPage,
  setFilterFirstName,
  setFilterLastName,
  setFilterRole,
  setSortParameter,
  resetFilters,
} = userManagementSlice.actions;

export default userManagementSlice.reducer;
