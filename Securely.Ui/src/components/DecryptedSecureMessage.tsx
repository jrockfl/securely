import React from 'react';

interface Props {
    message: string;
}

const DecryptedSecureMessage: React.FC<Props> = ({ message }) => {
    return (
        <div>
            <h2>Descrypted Message</h2>
            <p>{message}</p>
        </div>
    );
};

export default DecryptedSecureMessage;
