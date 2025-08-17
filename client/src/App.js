import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import TableContact from "./layout/TableContact/TableContact";
import {useEffect, useState} from "react";
import FormContact from "./layout/FormContact/FormContact";
import axios from "axios";
import {Route, Routes, useLocation} from "react-router-dom";
import ContactDetails from "./layout/ContactDetails/ContactDetails";
import Pagination from "./layout/Pagination/Pagination";

const baseApiUrl = process.env.REACT_APP_API_URL;

const App = () => {
    const [contacts, setContacts] = useState([]);
    const location = useLocation();
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(0);
    const [pageSize] = useState(10);

    useEffect(() => {
        const url = `${baseApiUrl}/contacts/page?pageNumber=${currentPage}&pageSize=${pageSize}`;
        axios.get(url)
            .then(
                res => {
                    setContacts(res.data.data);
                    setTotalPages(Math.ceil(res.data.totalCount / pageSize));
                }
            );
    }, [currentPage, pageSize, location.pathname]);

    const addContact = (contactName, contactEmail) => {
        const newId = contacts.length > 0
            ? Math.max(...contacts.map(contact => contact.id)) + 1
            : 1;
        const item = {
            id: newId,
            name: contactName,
            email: contactEmail
        }

        let url = `${baseApiUrl}/contacts`;
        axios.post(url, item);

        url = `${baseApiUrl}/contacts/page?pageNumber=${currentPage}&pageSize=${pageSize}`;
        axios.get(url).then(
            res => {
                setContacts(res.data.data);
                setTotalPages(Math.ceil(res.data.totalCount / pageSize));
            }
        )

    }

    const handlePageChange = (pageNumber) => {
        setCurrentPage(pageNumber);
    }

    return (
        <div className="container mt-5">
            <Routes>
                <Route path="/" element={
                    <div className="card">
                        <div className="card-header">
                            <h1>Список контактов</h1>
                        </div>

                        <div className="card-body">
                            <TableContact contacts={contacts}/>
                            <Pagination
                                currentPage={currentPage}
                                totalPages={totalPages}
                                onPageChange={handlePageChange}
                            />
                            <FormContact addContact={addContact}/>
                        </div>
                    </div>
                }/>
                <Route
                    path="/contact/:id"
                    element={<ContactDetails/>}
                />
            </Routes>
        </div>
    );
}

export default App;
