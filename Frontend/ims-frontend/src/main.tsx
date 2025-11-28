import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { Auth0Provider } from '@auth0/auth0-react';

const domain = import.meta.env.VITE_AUTH0_DOMAIN;
const clientId = import.meta.env.VITE_AUTH0_CLIENT_ID;
const audience = import.meta.env.VITE_AUTH0_AUDIENCE;
const scope = "openid profile email offline_access";

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Auth0Provider
      domain={domain}
      clientId={clientId}
      authorizationParams={{
        redirect_uri: globalThis.location.origin,
          audience: audience,
          scope: scope,
      }}
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
  </StrictMode>,
)
