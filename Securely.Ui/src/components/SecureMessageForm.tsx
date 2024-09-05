import React, { useState } from 'react';
import {
    EncryptionKeyData,
    createEncryptionKey,
} from '../services/EncryptionKeyApiService';
import {
    SecureMessageData,
    SecureMessageRequest,
    createMessage,
} from '../services/SecureMessageApiService';

import { useMutation } from '@tanstack/react-query';
import { encryptText } from '../utils/AesSecurity';
import SecureMessageCreated from './SecureMessageCreated';

const getTomorrowDate = () => {
    const today = new Date();
    const tomorrow = new Date(today.getTime() + 24 * 60 * 60 * 1000);
    const month = `${tomorrow.getUTCMonth() + 1}`.padStart(2, '0'); // getUTCMonth() returns month from 0-11
    const day = `${tomorrow.getUTCDate()}`.padStart(2, '0');
    const year = tomorrow.getUTCFullYear();
    return `${month}/${day}/${year}`; // Formats the date as MM/DD/YYYY
};

const SecureMessageForm: React.FC = () => {
    const [expirationDate, setExpirationDate] = useState<string>(
        getTomorrowDate(),
    );
    const [text, setText] = useState<string>('');
    const [secureMessageId, setSecureMessageId] = useState<string | null>(null);
    const [isMessageCreated, setIsMessageCreated] = useState<boolean>(false);

    const handleTextChange = (
        event: React.ChangeEvent<HTMLTextAreaElement>,
    ) => {
        setText(event.target.value);
    };

    const encryptionKeyMutation = useMutation<EncryptionKeyData>({
        mutationFn: createEncryptionKey,
    });

    const sendMessageMutation = useMutation<
        SecureMessageData,
        Error,
        SecureMessageRequest
    >({
        mutationFn: (requestData) =>
            createMessage(
                requestData.encryptionKeyId,
                requestData.secureMessage,
                requestData.createdBy,
                requestData.recipient,
                requestData.expirationDate,
            ),
    });

    const convertDate = (dateStr: string) => {
        const [month, day, year] = dateStr.split('/');
        return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
    };

    const handleEncryptAndSend = () => {
        encryptionKeyMutation.mutate(undefined, {
            onSuccess: (keyData) => {
                const key = keyData.key;
                const encryptedText = encryptText(text, key);

                sendMessageMutation.mutate(
                    {
                        encryptionKeyId: keyData.id,
                        secureMessage: encryptedText,
                        createdBy: 'system',
                        recipient: 'system',
                        expirationDate: convertDate(expirationDate),
                    },
                    {
                        onSuccess: (response) => {
                            setSecureMessageId(response.id);
                            setIsMessageCreated(true);
                        },
                        onError: (error) => {
                            console.error(
                                'Failed to send secure message.',
                                error,
                            );
                        },
                    },
                );
            },
            onError: (error) => {
                console.error('Failed to fetch key and encrypted text.', error);
            },
        });
    };

    if (
        encryptionKeyMutation.status === 'pending' ||
        sendMessageMutation.status === 'pending'
    )
        return <div>Loading...</div>;

    if (encryptionKeyMutation.status === 'error')
        return <div>Error {encryptionKeyMutation.error.message}</div>;
    if (sendMessageMutation.status === 'error')
        return <div>Error {sendMessageMutation.error.message}</div>;

    return (
        <div>
            {!isMessageCreated && (
                <div>
                    <h2>Secure Message</h2>
                    <input
                        type="text"
                        placeholder="Expiration Date"
                        value={expirationDate}
                        onChange={(e) => setExpirationDate(e.target.value)}
                    />
                    <br />
                    <textarea
                        value={text}
                        placeholder="Create secure message"
                        onChange={handleTextChange}
                        style={{ width: '300px', height: '100px' }}
                    />
                    <br />
                    <button onClick={handleEncryptAndSend}>
                        Create secure message
                    </button>
                </div>
            )}
            {secureMessageId && (
                <SecureMessageCreated secureMessageId={secureMessageId} />
            )}
        </div>
    );
};

export default SecureMessageForm;
