import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { Auth0Provider } from '@auth0/auth0-react';
import { Provider } from 'react-redux';
import { store } from './store.ts';

const domain = import.meta.env.VITE_AUTH0_DOMAIN;
const clientId = import.meta.env.VITE_AUTH0_CLIENT_ID;
const audience = import.meta.env.VITE_AUTH0_AUDIENCE;
const scope = "openid profile email offline_access";

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider store={store}>
      <Auth0Provider
        domain={domain}
        clientId={clientId}
        authorizationParams={{
          redirect_uri: globalThis.location.origin,
            audience: audience,
            scope: scope,
        }}
        useRefreshTokens={true}
        cacheLocation="localstorage"
        onRedirectCallback={appState => {
          globalThis.history.replaceState(
            {},
            document.title,
            appState?.returnTo ? appState.returnTo : globalThis.location.pathname
          );
        }}
      >
        <App />
      </Auth0Provider>
    </Provider>
  </StrictMode>,
)
