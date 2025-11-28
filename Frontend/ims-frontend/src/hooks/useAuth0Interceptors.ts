import { useEffect } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import type { AxiosInstance } from 'axios';

interface UseAuth0InterceptorsProps {
  axiosInstances: AxiosInstance[];
}

const useAuth0Interceptors = ({ axiosInstances }: UseAuth0InterceptorsProps) => {
  const { getAccessTokenSilently, isAuthenticated } = useAuth0();

  useEffect(() => {
    if (isAuthenticated) {
      const interceptorIds = axiosInstances.map(axiosInstance => {
        return axiosInstance.interceptors.request.use(async (config) => {
          try {
            const accessToken = await getAccessTokenSilently();
            config.headers.Authorization = `Bearer ${accessToken}`;
            return config;
          } catch (error) {
            console.error("Error getting access token", error);
            throw error;
          }
        });
      });

      return () => {
        axiosInstances.forEach((axiosInstance, index) => {
          axiosInstance.interceptors.request.eject(interceptorIds[index]);
        });
      };
    }
  }, [isAuthenticated, getAccessTokenSilently, axiosInstances]);
};

export default useAuth0Interceptors;
