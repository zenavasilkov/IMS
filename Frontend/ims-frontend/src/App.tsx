import './App.css';
import Auth0Login from './components/common/login/Auth0Login';
import useAuth0Interceptors from './hooks/useAuth0Interceptors';
import { ImsApi, RecruitmentApi } from './api/axios';
import LoginPage from './pages/LoginPage/LoginPage';
import { useAuth0 } from '@auth0/auth0-react';
import PageLoader from './components/common/pageLoader/PageLoader';
import useMinLoadingTime from "./hooks/useMinLoadingTime.ts";
import {useUserRole} from "./hooks/useUserRole.ts";
import {useMemo} from "react";
import PortalSwitcher from './components/portalSwitcher/PortalSwitcher';
import getPortalContent from "./components/PortalRouter/PortalRouter.tsx";
import {useSelector} from "react-redux";
import type {RootState} from "./store.ts";
import {useAppDispatch} from "./components/useAppDispatch.ts";
import {setAppActivePortal} from "./features/slices/appSlice.ts";

function App() {
  const { isAuthenticated, loginWithRedirect, isLoading } = useAuth0();
  const memoizedAxiosInstances = useMemo(() => [ImsApi, RecruitmentApi], []);
  const { isTokenLoading } = useAuth0Interceptors({ axiosInstances: memoizedAxiosInstances });
  const { role, isLoadingRole } = useUserRole();
  const isAppLoading = isLoading || isTokenLoading || isLoadingRole;
  const showPageLoader = useMinLoadingTime(isAppLoading, 1000);
  const showUnauthorizedPage = useMinLoadingTime(!isAuthenticated, 1000);
  const { activePortal } = useSelector((state: RootState) => state.app);
  const dispatch = useAppDispatch();

  const { content, headerTitle } = getPortalContent({role, activePortal});

  if (showPageLoader) {
    return <PageLoader loadingText="Loading authentication..." />;
  }

  if (showUnauthorizedPage) {
    return <div className="App"> <LoginPage onLogin={loginWithRedirect} /></div>
  }

  return (
    <div className="App">
      <nav className="Fixed-Header-Bar">
        <PortalSwitcher
            activePortal={activePortal}
            onSwitch={(portal) => dispatch(setAppActivePortal(portal))}
            role={role}
        />

        <div className="App-Title">{headerTitle}</div>
        <Auth0Login /> 
      </nav>

      <main className="App-Main-Content">{content}</main>
    </div>
  );
}

export default App;
