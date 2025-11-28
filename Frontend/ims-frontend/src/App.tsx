import './App.css'
import Auth0Login from './components/Auth0Login';
import useAuth0Interceptors from './hooks/useAuth0Interceptors';
import { ImsApi, RecruitmentApi } from './api/axios';

function App() {
  useAuth0Interceptors({ axiosInstances: [ImsApi, RecruitmentApi] });

  return (
    <div className="App">
      <header className="App-header">
        <Auth0Login />
      </header>
    </div>
  );
}

export default App;
