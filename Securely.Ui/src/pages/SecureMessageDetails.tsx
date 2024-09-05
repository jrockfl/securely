import { useParams } from 'react-router-dom';
import SecureMessageConfirmation from '../components/SecureMessageConfirmation';

const SecureMessageDetails: React.FC = () => {
    const { id } = useParams<{ id?: string }>();

    if (!id) return <div>No id provided</div>;
    return <SecureMessageConfirmation id={id} />;
};

export default SecureMessageDetails;
