import { useEffect, useState } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import type { AxiosInstance } from 'axios';

interface UseAuth0InterceptorsProps {
  axiosInstances: AxiosInstance[];
}

const apiAudience = import.meta.env.VITE_AUTH0_AUDIENCE;
let isMounted : boolean;


const useAuth0Interceptors = ({ axiosInstances }: UseAuth0InterceptorsProps) => {
  const { getAccessTokenSilently, isAuthenticated } = useAuth0();
  const [isTokenLoading, setIsTokenLoading] = useState(false);

  const interceptorIds = axiosInstances.map(axiosInstance => {
    return axiosInstance.interceptors.request.use(async (config) => {
      const accessToken = await getAccessTokenSilently({
        authorizationParams: {
          audience: apiAudience,
        }
      });
      config.headers.Authorization = `Bearer ${accessToken}`;
      return config;
    });
  });

  const setupInterceptors = async () => {
    try {
      await getAccessTokenSilently({
        authorizationParams: {
          audience: apiAudience,
        },
      });
      if (isMounted) {
        setIsTokenLoading(false);
      }
    } catch (error) {
      console.error("Error pre-fetching access token", error);
      if (isMounted) {
        setIsTokenLoading(false);
      }
    }
    
    return () => {
      isMounted = false;
      axiosInstances.forEach((axiosInstance, index) => {
        axiosInstance.interceptors.request.eject(interceptorIds[index]);
      });
    };
  };

  useEffect(() => {
    isMounted = true;

    if (!isAuthenticated) {
      setIsTokenLoading(false);
      return;
    }

    setIsTokenLoading(true);

    setupInterceptors();

    return () => {
      isMounted = false;
    };
  }, [isAuthenticated, getAccessTokenSilently, axiosInstances, apiAudience]);

  return { isTokenLoading };
};

export default useAuth0Interceptors;
