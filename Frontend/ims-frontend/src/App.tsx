import './App.css';
import Auth0Login from './components/common/login/Auth0Login';
import useAuth0Interceptors from './hooks/useAuth0Interceptors';
import { ImsApi, RecruitmentApi } from './api/axios';
import LoginPage from './pages/LoginPage/LoginPage';
import { useAuth0 } from '@auth0/auth0-react';
import PageLoader from './components/common/pageLoader/PageLoader'; 

import useHasPermission from './hooks/useHasPermissions';
import UserManagementPage from './pages/UserManagementPage/UserManagementPage';
import useMinLoadingTime from "./hooks/useMinLoadingTime.ts";


function App() {
  const { isAuthenticated, loginWithRedirect, isLoading } = useAuth0();
  const { isTokenLoading } = useAuth0Interceptors({ axiosInstances: [ImsApi, RecruitmentApi] });
  const { hasPermission: canManageUsers, isLoadingPermissions } = useHasPermission('create:users');
  const isAppLoading = isLoading || isLoadingPermissions || isTokenLoading;
  const showPageLoader = useMinLoadingTime(isAppLoading, 300);

  if (showPageLoader) {
      return <PageLoader loadingText="Loading authentication..." />;
  }

  if (!isAuthenticated) {
    return (
      <div className="App">
        <LoginPage onLogin={loginWithRedirect} />
      </div>
    );
  }

  return (
    <div className="App">
      <nav className="Fixed-Header-Bar">
        <div className="App-Title"></div>
        <Auth0Login /> 
      </nav>

      <main className="App-Main-Content">
        {canManageUsers ? (
          <UserManagementPage />
        ) : (
          <div style={{ marginTop: '20px' }}>
              <h2>Welcome to the IMS Portal</h2>
              <p>You are logged in, but you do not have permission to access the User Management section.</p> 
          </div>
        )}
      </main>
    </div>
  );
}

export default App;
