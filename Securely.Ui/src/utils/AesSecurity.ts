import CryptoJS from 'crypto-js';

export const encryptText = (text: string, key: string): string => {
    try {
        return CryptoJS.AES.encrypt(text, key).toString();
    } catch (error) {
        console.error('Encryption failed: ', error);
        throw error;
    }
};

export const decryptText = (text: string, key: string): string => {
    try {
        const bytes = CryptoJS.AES.decrypt(text, key);
        return bytes.toString(CryptoJS.enc.Utf8);
    } catch (error) {
        console.error('Decryption faild: ', error);
        throw error;
    }
};
