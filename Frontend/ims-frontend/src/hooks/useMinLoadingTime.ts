import { useState, useEffect } from 'react';

const useMinLoadingTime = (isLoading: boolean, minDelayMs: number = 500): boolean => {
    const [isDelayMet, setIsDelayMet] = useState(false);

    useEffect(() => {
        let timer: ReturnType<typeof setTimeout> | undefined;

        if (isLoading) {
            setIsDelayMet(false);
            timer = setTimeout(() => setIsDelayMet(true), minDelayMs);
        }
        
        return () => {
            if (timer !== undefined) {
                clearTimeout(timer);
            }
        };
    }, [isLoading, minDelayMs]);
    
    return isLoading && isDelayMet;
};

export default useMinLoadingTime;
