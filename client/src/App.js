import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import TableContact from "./layout/TableContact/TableContact";
import {useState} from "react";
import FormContact from "./layout/FormContact/FormContact";

const App = () => {
    const [contacts, setContacts] = useState(
        Array.from({length: 5}, (_, i) => (
            {
                id: i + 1,
                name: `Имя фамилия ${i + 1}`,
                email: `email${i + 1}@example.com`
            }
        ))
    )

    const addContact = (contactName, contactEmail) => {
        const newId = Math.max(...contacts.map(contact => contact.id)) + 1;
        const item = {
            id: newId,
            name: contactName,
            email: contactEmail
        }
        setContacts([...contacts, item]);
    }

    return (
        <div className="container mt-5">
            <div className="card">
                <div className="card-header">
                    <h1>Список контактов</h1>
                </div>

                <div className="card-body">
                    <TableContact contacts={contacts}/>
                    <FormContact addContact={addContact}/>
                </div>
            </div>
        </div>
    );
}

export default App;
