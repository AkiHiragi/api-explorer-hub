import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import TableContact from "./layout/TableContact/TableContact";
import {useState} from "react";

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

    function addContact() {
        let index = contacts.length + 1;
        const item = {
            id: index,
            name: `Имя фамилия ${index}`,
            email: `email${index}@example.com`
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
                    <div>
                        <button
                            className="btn btn-primary"
                            onClick={() => {
                                addContact()
                            }}
                        >
                            Добавить контакт
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default App;
