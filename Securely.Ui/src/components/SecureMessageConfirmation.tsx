import { useMutation, useQuery } from '@tanstack/react-query';
import React, { useEffect, useState } from 'react';
import {
    EncryptionKeyData,
    getEncryptionKeyByMessageId,
} from '../services/EncryptionKeyApiService';
import {
    CleanupRequestData,
    sendCleanupRequest,
} from '../services/CleanupApiService';
import { getMessage } from '../services/SecureMessageApiService';
import { decryptText } from '../utils/AesSecurity';
import DecryptedSecureMessage from './DecryptedSecureMessage';
import { useNavigate } from 'react-router-dom';

interface Props {
    id: string;
}

const SecureMessageConfirmation: React.FC<Props> = ({ id }) => {
    const [decryptedMessage, setDecryptedMessage] = useState<string | null>(
        null,
    );
    const [isDecrypting, setIsDecrypting] = useState<boolean>(false);
    const [isMessageHandled, setIsMessageHandled] = useState<boolean>(false);

    const navigate = useNavigate();

    const { data, isLoading, isError, error } = useQuery({
        queryKey: ['getMessage', id],
        queryFn: () => getMessage(id),
        retry: false,
        enabled: !isMessageHandled,
    });

    const encryptionKeyMutation = useMutation<EncryptionKeyData, Error, string>(
        {
            mutationFn: () => getEncryptionKeyByMessageId(id),
            onMutate: () => setIsDecrypting(true),
        },
    );

    const cleanupMutation = useMutation<
        void,
        Error,
        { secureMessageId: string; encryptionKeyId: string }
    >({
        mutationFn: (payload) =>
            sendCleanupRequest(
                payload.secureMessageId,
                payload.encryptionKeyId,
            ),
    });

    const handleYesClick = () => {
        if (isMessageHandled) return;

        encryptionKeyMutation.mutate(id, {
            onSuccess: (encryptionKeyData) => {
                if (data?.message) {
                    const message = decryptText(
                        data?.message,
                        encryptionKeyData.key,
                    );
                    setDecryptedMessage(message);
                    setIsDecrypting(false);
                    setIsMessageHandled(true);

                    cleanupMutation.mutate({
                        secureMessageId: id,
                        encryptionKeyId: encryptionKeyData.id,
                    });
                }
            },
            onError(error) {
                console.error('Failed to fetch encryption key', error);
                setIsDecrypting(false);
            },
        });
    };

    const handleNoClick = () => {
        navigate('/');
    };

    if (isLoading) {
        return <div>Loading...</div>;
    }

    if (isError) {
        return <div>{error.message}</div>;
    }

    return (
        <div>
            {isDecrypting ? (
                <div>Loading...</div>
            ) : decryptedMessage ? (
                <DecryptedSecureMessage message={decryptedMessage} />
            ) : (
                <div>
                    <h2>Secure Message</h2>
                    <h3>Read and destroy?</h3>
                    <p>
                        You're about to read and destroy the message with id{' '}
                        {id}.
                    </p>
                    <button onClick={handleYesClick}>
                        Yes, show me the message
                    </button>
                    <button onClick={handleNoClick}>No, not now</button>
                </div>
            )}
        </div>
    );
};

export default SecureMessageConfirmation;
