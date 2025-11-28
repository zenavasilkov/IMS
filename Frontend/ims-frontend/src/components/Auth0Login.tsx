import React from 'react';
import { useAuth0 } from '@auth0/auth0-react';

const Auth0Login: React.FC = () => {
  const { loginWithRedirect, logout, user, isAuthenticated, isLoading } = useAuth0();

  if (isLoading) {
    return <div>Loading authentication...</div>;
  }

  return (
    <div>
      {!isAuthenticated && (
        <button onClick={() => loginWithRedirect()}>Log In</button>
      )}
      {isAuthenticated && (
        <div>
          <img src={user?.picture} alt={user?.name} />
          <h2>{user?.name}</h2>
          <p>{user?.email}</p>
          <button onClick={() => logout({ logoutParams: { returnTo: window.location.origin } })}>Log Out</button>
        </div>
      )}
    </div>
  );
};

export default Auth0Login;
