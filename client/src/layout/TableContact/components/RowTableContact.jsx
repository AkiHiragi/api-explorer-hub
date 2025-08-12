const RowTableContact = (props) => {
    return (
        <tr
            onClick={() => {
                props.deleteContact(props.id)
            }}
        >
            <td>{props.id}</td>
            <td>{props.name}</td>
            <td>{props.email}</td>
        </tr>
    )
}

export default RowTableContact;