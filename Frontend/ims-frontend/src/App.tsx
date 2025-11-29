import './App.css'
import Auth0Login from './components/Auth0Login';
import useAuth0Interceptors from './hooks/useAuth0Interceptors';
import { ImsApi, RecruitmentApi } from './api/axios';
import LoginPage from './pages/LoginPage/LoginPage';
import { useAuth0 } from '@auth0/auth0-react';
import PageLoader from './components/common/pageLoader/PageLoader'; 

function App() {
  const { isAuthenticated, loginWithRedirect, isLoading } = useAuth0();
  useAuth0Interceptors({ axiosInstances: [ImsApi, RecruitmentApi] });

  if (isLoading) {
    return <PageLoader />;
  }

  return (
    <div className="App">
      {isAuthenticated ? (
        <header className="App-header">
          <Auth0Login />
        </header>
      ) : (
        <LoginPage onLogin={loginWithRedirect} />
      )}
    </div>
  );
}

export default App;
