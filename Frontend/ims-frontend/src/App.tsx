import './App.css';
import Auth0Login from './components/common/login/Auth0Login';
import useAuth0Interceptors from './hooks/useAuth0Interceptors';
import { ImsApi, RecruitmentApi } from './api/axios';
import LoginPage from './pages/LoginPage/LoginPage';
import { useAuth0 } from '@auth0/auth0-react';
import PageLoader from './components/common/pageLoader/PageLoader';
import UserManagementPage from './pages/UserManagementPage/UserManagementPage';
import useMinLoadingTime from "./hooks/useMinLoadingTime.ts";
import {ManagementIcon} from "./components/common/Icons.tsx";
import {useUserRole} from "./hooks/useUserRole.ts";

function App() {
  const { isAuthenticated, loginWithRedirect, isLoading } = useAuth0();
  const { isTokenLoading } = useAuth0Interceptors({ axiosInstances: [ImsApi, RecruitmentApi] });
  const { role, isLoadingRole } = useUserRole();
  const isAppLoading = isLoading || isTokenLoading || isLoadingRole;
  const showPageLoader = useMinLoadingTime(isAppLoading, 300);
  const HrManagerHeader = () => <div><ManagementIcon className="App-Header-Icon" />User Management Portal</div>;

  let contentToRender;
  let header;

  if (role === 'HRManager'){
      contentToRender = <UserManagementPage />;
      header = <HrManagerHeader />
  }

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
          <div className="App-Title">{ header }</div>
        <Auth0Login /> 
      </nav>

      <main className="App-Main-Content">
          {contentToRender}
      </main>
    </div>
  );
}

export default App;
