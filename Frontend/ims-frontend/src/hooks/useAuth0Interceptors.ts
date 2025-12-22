import { useEffect, useState, useRef } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import type { AxiosInstance } from 'axios';

interface UseAuth0InterceptorsProps {
  axiosInstances: AxiosInstance[];
}

const apiAudience = import.meta.env.VITE_AUTH0_AUDIENCE;

const useAuth0Interceptors = ({ axiosInstances }: UseAuth0InterceptorsProps) => {
  const { getAccessTokenSilently, isAuthenticated } = useAuth0();
  const [isTokenLoading, setIsTokenLoading] = useState(isAuthenticated);
  const isMountedRef = useRef(true);

  const preFetchTokenAndResolve = async () => {
    try {
      await getAccessTokenSilently({
        authorizationParams: { audience: apiAudience },
      });
      if (isMountedRef.current) {
        setIsTokenLoading(false);
      }
    } catch (error) {
      console.error("Error pre-fetching access token", error);
      if (isMountedRef.current) {
        setIsTokenLoading(false);
      }
    }
  };


  useEffect(() => {
    isMountedRef.current = true;

    if (!isAuthenticated) {
      setIsTokenLoading(false);
      return;
    }

    if (isTokenLoading) {
      setIsTokenLoading(true);
    }

    const interceptorIds = axiosInstances.map(axiosInstance => {
      return axiosInstance.interceptors.request.use(async (config) => {
        const accessToken = await getAccessTokenSilently({
          authorizationParams: { audience: apiAudience },
        });
        config.headers.Authorization = `Bearer ${accessToken}`;
        return config;
      });
    });

    preFetchTokenAndResolve();

    return () => {
      isMountedRef.current = false;
      axiosInstances.forEach((axiosInstance, index) => {
        axiosInstance.interceptors.request.eject(interceptorIds[index]);
      });
    };
  }, [isAuthenticated, getAccessTokenSilently, axiosInstances, apiAudience]);

  return { isTokenLoading };
};

export default useAuth0Interceptors;
