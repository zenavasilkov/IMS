import { useState, useEffect } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import {decodeJwt} from "../features/JwtDecoder.ts";
import useAuth0Interceptors from "./useAuth0Interceptors.ts";

const ROLE_CLAIM_KEY = import.meta.env.VITE_ROLE_CLAIM_KEY;
const AUDIENCE = import.meta.env.VITE_AUTH0_AUDIENCE;

export const useUserRole = (): { role: string | null, isLoadingRole: boolean } => {
    const { getAccessTokenSilently, isAuthenticated } = useAuth0();
    const [role, setRole] = useState<string | null>(null);
    const { isTokenLoading } = useAuth0Interceptors({ axiosInstances: [] })
    const [isLoadingRole, setIsLoadingRole] = useState(isAuthenticated || isTokenLoading);

    useEffect(() => {
        let isMounted = true;

        const getRole = async () => {
            if (!isAuthenticated) {
                if (isMounted) {
                    setRole(null);
                    setIsLoadingRole(false);
                }
                return;
            }

            try {
                const accessToken = await getAccessTokenSilently({
                    authorizationParams: {
                        audience: AUDIENCE,
                    },
                });

                const decodedToken = decodeJwt(accessToken);

                const roles: string[] = decodedToken?.[ROLE_CLAIM_KEY] || [];
                const primaryRole = roles.length > 0 ? roles[0] : null;

                if (isMounted) {
                    setRole(primaryRole);
                    setIsLoadingRole(false);
                }
            } catch (error) {
                console.error("Error retrieving user role:", error);
                if (isMounted) {
                    setRole(null);
                    setIsLoadingRole(false);
                }
            }
        };

        getRole();

        return () => { isMounted = false; };
    }, [isAuthenticated, isTokenLoading, getAccessTokenSilently, AUDIENCE]);

    return { role, isLoadingRole };
};
