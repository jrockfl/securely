interface Props {
    secureMessageId: string | null;
}

const SecureMessageCreated: React.FC<Props> = ({ secureMessageId }) => {
    const baseUrl = process.env.REACT_APP_BASE_URL;
    if (!baseUrl) throw new Error('Base url is not defined.');
    const secureMessageUrl = `${baseUrl}/${secureMessageId}`;

    return (
        <div>
            <h2>Message created successfully</h2>
            <p>Your message has been encrypted</p>
            <p>
                <a href={secureMessageUrl}>{secureMessageUrl}</a>
            </p>
        </div>
    );
};

export default SecureMessageCreated;
