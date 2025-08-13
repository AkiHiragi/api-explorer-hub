import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import TableContact from "./layout/TableContact/TableContact";
import {useEffect, useState} from "react";
import FormContact from "./layout/FormContact/FormContact";
import axios from "axios";

const baseApiUrl = process.env.REACT_APP_API_URL;

const App = () => {
    const url = `${baseApiUrl}/contacts`;
    const [contacts, setContacts] = useState([])

    useEffect(() => {
        axios.get(url)
            .then(
                res => setContacts(res.data)
            );
    }, []);

    const addContact = (contactName, contactEmail) => {
        const newId = contacts.length > 0
            ? Math.max(...contacts.map(contact => contact.id)) + 1
            : 1;
        const item = {
            id: newId,
            name: contactName,
            email: contactEmail
        }
        
        const url = `${baseApiUrl}/contacts`;
        axios.post(url, item);
        setContacts([...contacts, item]);
    }

    const deleteContact = (id) => {
        const url = `${baseApiUrl}/contacts/${id}`;
        axios.delete(url);
        setContacts(contacts.filter(contact => contact.id !== id));
    }

    return (
        <div className="container mt-5">
            <div className="card">
                <div className="card-header">
                    <h1>Список контактов</h1>
                </div>

                <div className="card-body">
                    <TableContact
                        contacts={contacts}
                        deleteContact={deleteContact}
                    />
                    <FormContact addContact={addContact}/>
                </div>
            </div>
        </div>
    );
}

export default App;
