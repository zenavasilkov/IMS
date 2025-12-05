import { useState, useEffect } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import useAuth0Interceptors from './useAuth0Interceptors';

const audience = import.meta.env.VITE_AUTH0_AUDIENCE;

const decodeJwt = (token: string) => {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
  } catch (e) {
    console.error("Failed to decode JWT:", e);
    return null;
  }
};

const useHasPermission = (permission: string): { hasPermission: boolean, isLoadingPermissions: boolean } => {
  const { isTokenLoading } = useAuth0Interceptors({ axiosInstances: [] });
  const { getAccessTokenSilently, isAuthenticated } = useAuth0();
  const [hasPermission, setHasPermission] = useState(false);
  const [isLoadingPermissions, setIsLoadingPermissions] = useState(isAuthenticated || isTokenLoading);

  useEffect(() => {
    let isMounted = true;

    if (!isAuthenticated || isTokenLoading) { 
      if (isMounted && !isTokenLoading) {
          setIsLoadingPermissions(false);
      }
      return;
    }
    
    const checkPermission = async () => {
      if (!isAuthenticated) {
        if (isMounted) {
          setHasPermission(false);
          setIsLoadingPermissions(false);
        }
        return;
      }

      try {
        const accessToken = await getAccessTokenSilently({
          authorizationParams: {
              audience: audience, 
          }
        });
        
        const decodedToken = decodeJwt(accessToken);
        
        if (isMounted) {
          const userPermissions: string[] = decodedToken?.permissions || [];
          setHasPermission(userPermissions.includes(permission));
          setIsLoadingPermissions(false);
        }

      } catch (error) {
        console.error(`Error checking permission '${permission}':`, error);
        if (isMounted) {
          setHasPermission(false);
          setIsLoadingPermissions(false);
        }
      }
    };

    checkPermission();

    return () => {
      isMounted = false;
    };
  }, [isAuthenticated, isTokenLoading, getAccessTokenSilently, permission]);

  return { hasPermission, isLoadingPermissions };
};

export default useHasPermission;
