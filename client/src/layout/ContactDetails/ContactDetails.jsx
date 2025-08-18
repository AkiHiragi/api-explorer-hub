import {useEffect, useState} from "react";
import {useNavigate, useParams} from "react-router-dom";
import axios from "axios";

const baseApiUrl = process.env.REACT_APP_API_URL;
const ContactDetails = ({onUpdate}) => {
    const [contact, setContact] = useState({id: "", name: "", email: ""});
    const {id} = useParams();
    const navigate = useNavigate();

    useEffect(() => {
            const url = `${baseApiUrl}/contacts/${id}`;
            axios.get(url).then(
                response => setContact(response.data)
            ).catch(
                _ => navigate("/")
            )
        }, [id, navigate]
    )

    const handleDelete = () => {
        const url = `${baseApiUrl}/contacts/${id}`;
        if (window.confirm(`Вы действительно хотите удалить контакт ${contact.name}?`)) {
            axios.delete(url).then(() => {
                onUpdate();
                navigate("/")
            });
        }
    }

    const handleUpdate = () => {
        const url = `${baseApiUrl}/contacts/${id}`;
        if (contact.name.trim() !== "" && contact.email.trim() !== "") {
            axios.put(url, contact).then(() => {
                onUpdate();
                navigate("/");
            });
        } else
            alert("Пожалуйста введите корректные данные")
    }

    return (
        <div className="container mt-5">
            <h2>Детали контакта</h2>
            <div className="mb-3">
                <label className="form-label">Имя: </label>
                <input
                    className="form-control"
                    type="text"
                    value={contact.name}
                    onChange={(e) => {
                        setContact({...contact, name: e.target.value});
                    }}
                />
            </div>
            <div className="mb-3">
                <label className="form-label">Email: </label>
                <input
                    className="form-control"
                    type="email"
                    value={contact.email}
                    onChange={(e) => {
                        setContact({...contact, email: e.target.value});
                    }}
                />
            </div>
            <button className="btn btn-primary me-2" onClick={handleUpdate}>
                Обновить
            </button>

            <button className="btn btn-danger me-2" onClick={handleDelete}>
                Удалить
            </button>

            <button className="btn btn-secondary me-2" onClick={() => {
                navigate("/")
            }}>
                Назад
            </button>
        </div>
    )
}

export default ContactDetails;