import './App.css';
import Auth0Login from './components/Auth0Login';
import useAuth0Interceptors from './hooks/useAuth0Interceptors';
import { ImsApi, RecruitmentApi } from './api/axios';
import LoginPage from './pages/LoginPage/LoginPage';
import { useAuth0 } from '@auth0/auth0-react';
import PageLoader from './components/common/pageLoader/PageLoader'; 

import useHasPermission from './hooks/useHasPermissions';
import UserManagementPage from './pages/UserManagementPage/UserManagementPage';


function App() {
  const { isAuthenticated, loginWithRedirect, isLoading } = useAuth0();
  const { isTokenLoading } = useAuth0Interceptors({ axiosInstances: [ImsApi, RecruitmentApi] });

  const { hasPermission: canManageUsers, isLoadingPermissions } = useHasPermission('create:users');

  if (isLoading || isLoadingPermissions || isTokenLoading) {
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
      <header className="App-header">
        <Auth0Login /> 
        {canManageUsers ? (
          <UserManagementPage />
        ) : (
          <div style={{ marginTop: '20px' }}>
              <h2>Welcome to the IMS Portal</h2>
              <p>You are logged in, but you do not have permission to access the User Management section.</p> 
          </div>
        )}
        
      </header>
    </div>
  );
}

export default App;
