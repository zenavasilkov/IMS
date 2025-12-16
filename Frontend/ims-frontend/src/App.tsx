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
import {useState, useMemo} from "react";
import RecruitmentPage from "./pages/RecruitmentManagementPage/RecruitmentPage.tsx";
import PortalSwitcher, { PORTALS } from './components/portalSwitcher/PortalSwitcher';
import DepartmentPage from "./pages/DepartmentPage/DepartmentPage.tsx";
import EmployeePage from "./pages/EmployeePage/EmployeePage.tsx";
import InterviewPage from "./pages/InterviewPage/InterviewPage.tsx";
import InternshipPage from "./pages/InternshipPage/InternshipPage.tsx";

const HrManagerHeader = (text : string) => <div><ManagementIcon className="App-Header-Icon" />{text}</div>;

function App() {
  const { isAuthenticated, loginWithRedirect, isLoading } = useAuth0();
  const memoizedAxiosInstances = useMemo(() => [ImsApi, RecruitmentApi], []);
  const { isTokenLoading } = useAuth0Interceptors({ axiosInstances: memoizedAxiosInstances });
  const { role, isLoadingRole } = useUserRole();
  const isAppLoading = isLoading || isTokenLoading || isLoadingRole;
  const showPageLoader = useMinLoadingTime(isAppLoading, 1000);
  const [activePortal, setActivePortal] = useState(PORTALS.USER_MANAGEMENT);
  const HrManagerRole = 'HRManager';

  let contentToRender;
  let headerTitle;

  if (role === HrManagerRole) {
    if (activePortal === PORTALS.RECRUITMENT) {
      contentToRender = <RecruitmentPage />;
      headerTitle = HrManagerHeader("Recruitment Management");
    } else if (activePortal === PORTALS.INTERVIEWS) {
      contentToRender = <InterviewPage />;
      headerTitle = HrManagerHeader("Interview Management");
    } else if (activePortal === PORTALS.USER_MANAGEMENT) {
      contentToRender = <UserManagementPage />;
      headerTitle = HrManagerHeader("User Management");
    } else if (activePortal === PORTALS.DEPARTMENTS) {
      contentToRender = <DepartmentPage />;
      headerTitle = HrManagerHeader("Department Management");
    } else if (activePortal === PORTALS.EMPLOYEES) {
      contentToRender = <EmployeePage />;
      headerTitle = HrManagerHeader("Employee Management");
    } else if (activePortal === PORTALS.INTERNSHIPS) {
      contentToRender = <InternshipPage/>;
      headerTitle = HrManagerHeader("Internship Management");
    }
  } else {
    contentToRender = <div className="No-Access">Access Denied: Your role does not allow portal switching.</div>;
    headerTitle = "IMS Portal";
  }

  if (showPageLoader) {
    return <PageLoader loadingText="Loading authentication..." />;
  }

  if (!isAuthenticated) {
    return <div className="App"> <LoginPage onLogin={loginWithRedirect} /></div>
  }

  return (
    <div className="App">
      <nav className="Fixed-Header-Bar">
          <PortalSwitcher
              activePortal={activePortal}
              onSwitch={setActivePortal}
              isVisible={role === HrManagerRole}
          />

          <div className="App-Title">{headerTitle}</div>
        <Auth0Login /> 
      </nav>

      <main className="App-Main-Content">{contentToRender}</main>
    </div>
  );
}

export default App;
