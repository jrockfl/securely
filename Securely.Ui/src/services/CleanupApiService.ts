import axios, { AxiosResponse } from 'axios';
import { error } from 'console';

export interface CleanupRequestData {
    secureMessageId: string;
    encryptionKeyId: string;
}

export async function sendCleanupRequest(
    secureMessageId: string,
    encryptionKeyId: string,
): Promise<void> {
    const apiUrl = process.env.REACT_APP_API_URL;
    if (!apiUrl) throw new Error('Api url is not defined.');

    const postData: CleanupRequestData = {
        encryptionKeyId: encryptionKeyId,
        secureMessageId: secureMessageId,
    };

    try {
        const response: AxiosResponse<void> = await axios.post(
            `${apiUrl}/api/cleanup`,
            postData,
        );

        if (response.status !== 202) {
            throw new Error(`Unexpected response status ${response.status}.`);
        }
    } catch (error: any) {
        const serverResponse = error.response;
        const statusCode = serverResponse.status;
        console.error(error);
        throw new Error(
            `An error has occurred with status code ${statusCode}.`,
        );
    }
}
