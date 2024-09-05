import axios, { AxiosResponse, AxiosError } from 'axios';

export interface SecureMessageData {
    id: string;
    message: string;
}

interface SecureMessageResponse {
    data: SecureMessageData;
}

export interface SecureMessageRequest {
    encryptionKeyId: string;
    secureMessage: string;
    createdBy: string;
    recipient: string;
    expirationDate: string;
}

export async function createMessage(
    encryptionKeyId: string,
    secureMessage: string,
    createdBy: string,
    recipient: string,
    expirationDate: string,
): Promise<SecureMessageData> {
    const apiUrl = process.env.REACT_APP_API_URL;

    if (!apiUrl) throw new Error('Api url is not defined.');

    const postData: SecureMessageRequest = {
        encryptionKeyId: encryptionKeyId,
        secureMessage: secureMessage,
        createdBy: createdBy,
        recipient: recipient,
        expirationDate: expirationDate,
    };

    try {
        const response: AxiosResponse<SecureMessageResponse> = await axios.post(
            `${apiUrl}/api/secureMessages`,
            postData,
        );
        return response.data.data;
    } catch (error: any) {
        const serverResponse = error.response;
        const statusCode = serverResponse.status;
        console.error(error);
        throw new Error(
            `An error has occurred with status code ${statusCode}.`,
        );
    }
}

export async function getMessage(
    messageId: string,
): Promise<SecureMessageData> {
    const apiUrl = process.env.REACT_APP_API_URL;

    if (!apiUrl) throw new Error('Api url is not defined.');

    try {
        const response: AxiosResponse<SecureMessageResponse> = await axios.get(
            `${apiUrl}/api/secureMessages/${messageId}`,
        );
        return response.data.data;
    } catch (error: any) {
        const serverResponse = error.response;
        const statusCode = serverResponse.status;
        console.error(error);

        if (serverResponse.status === 404) {
            throw new Error('The requested message does not exist.');
        }

        if (serverResponse.status === 410) {
            throw new Error(
                'The requested message is no longer available. It has expired or has already been read.',
            );
        }

        throw new Error(
            `An error has occurred with status code ${statusCode}.`,
        );
    }
}
