import axios, { AxiosResponse } from 'axios';

export interface EncryptionKeyData {
    id: string;
    key: string;
}

interface EncryptionKeyResponse {
    data: EncryptionKeyData;
}

export async function createEncryptionKey(): Promise<EncryptionKeyData> {
    const apiUrl = process.env.REACT_APP_API_URL;

    if (!apiUrl) throw new Error('Api url is not defined.');

    try {
        const response: AxiosResponse<EncryptionKeyResponse> = await axios.post(
            `${apiUrl}/api/encryptionKeys`,
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

export async function getEncryptionKey(id: string) {
    const apiUrl = process.env.REACT_APP_API_URL;

    if (!apiUrl) throw new Error('Api url is not defined.');

    try {
        const response: AxiosResponse<EncryptionKeyResponse> = await axios.get(
            `${apiUrl}/api/encryptionKeys/${id}`,
        );
        return response.data.data;
    } catch (error: any) {
        const serverResponse = error.response;
        const statusCode = serverResponse.status;
        console.error(error);

        if (serverResponse.status === 404) {
            throw new Error('The requested encryption key does not exist.');
        }

        throw new Error(
            `An error has occurred with status code ${statusCode}.`,
        );
    }
}

export async function getEncryptionKeyByMessageId(messageId: string) {
    const apiUrl = process.env.REACT_APP_API_URL;

    if (!apiUrl) throw new Error('Api url is not defined.');

    try {
        const response: AxiosResponse<EncryptionKeyResponse> = await axios.get(
            `${apiUrl}/api/secureMessages/${messageId}/encryptionKey/`,
        );
        return response.data.data;
    } catch (error: any) {
        const serverResponse = error.response;
        const statusCode = serverResponse.status;
        console.error(error);

        if (serverResponse.status === 404) {
            throw new Error('The requested encryption key does not exist.');
        }

        throw new Error(
            `An error has occurred with status code ${statusCode}.`,
        );
    }
}
