import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './App.css';
import SecureMessage from './pages/SecureMessage';
import SecureMessageDetails from './pages/SecureMessageDetails';

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<SecureMessage />} />
                <Route path="/:id" element={<SecureMessageDetails />} />
            </Routes>
        </Router>
    );
}

export default App;
