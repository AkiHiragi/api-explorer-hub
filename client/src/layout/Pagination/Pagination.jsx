import {useState} from "react";

const Pagination = ({currentPage, totalPages, onPageChange}) => {
    const [isEditing, setIsEditing] = useState(false);
    const [inputValue, setInputValue] = useState("");

    const pageNumber = [];
    let startPage, endPage;
    const countPagination = 8;

    if (totalPages <= countPagination) {
        startPage = 1;
        endPage = totalPages;
    } else {
        if (currentPage <= countPagination - 4) {
            startPage = 1;
            endPage = countPagination;
        } else if (currentPage + 4 >= totalPages) {
            startPage = totalPages - (countPagination - 1);
            endPage = totalPages;
        } else {
            startPage = currentPage - (Math.floor(countPagination / 2 - 1));
            endPage = currentPage + (Math.floor(countPagination / 2 - 1));
        }
    }

    const handleInputSubmit = (e) => {
        e.preventDefault();
        const num = parseInt(inputValue, 10);
        if (!isNaN(num) && num >= 1 && num <= totalPages) {
            onPageChange(num);
        }
        setIsEditing(false);
        setInputValue("");
    }

    for (let index = startPage; index <= endPage; index++) {
        pageNumber.push(index);
    }

    return (
        <nav>
            <ul className="pagination">
                <li className={`page-item ${currentPage === 1 ? "disabled" : ""}`}>
                    <button
                        className="page-link"
                        onClick={() => {
                            onPageChange(currentPage - 1);
                        }}
                        disabled={currentPage === 1}
                    >
                        Предыдущая
                    </button>
                </li>

                {startPage > 1 && (
                    <>
                        <li className="page-item">
                            <button
                                className="page-link"
                                onClick={() => {
                                    onPageChange(1)
                                }}>
                                1
                            </button>
                        </li>
                    {startPage > 2 && (
                        <li className="page-item">
                            {isEditing ? (
                                <form onSubmit={handleInputSubmit}>
                                    <input
                                        type="number"
                                        min="1"
                                        max={totalPages}
                                        value={inputValue}
                                        onChange={(e) => setInputValue(e.target.value)}
                                        onBlur={() => setIsEditing(false)}
                                        className="form-control"
                                        style={{width: "70px"}}
                                        autoFocus
                                    />
                                </form>
                            ) : (
                                <button
                                    className="page-link"
                                    onClick={() => {
                                        setIsEditing(true)
                                    }}
                                >
                                    ...
                                </button>)}
                        < /li>
                    )}
                    </>
                )}

                {pageNumber.map(pageIndex => (
                    <li key={pageIndex}
                        className={`page-item ${currentPage === pageIndex ? "disabled" : ""}`}>
                        <button
                            className="page-link"
                            onClick={() => {
                                onPageChange(pageIndex);
                            }}>
                            {pageIndex}
                        </button>
                    </li>
                ))}

                {endPage < totalPages && (
                    <>
                        {endPage < totalPages - 1 && (
                            <li className="page-item">
                                {isEditing ? (
                                    <form onSubmit={handleInputSubmit}>
                                        <input
                                            type="number"
                                            min="1"
                                            max={totalPages}
                                            value={inputValue}
                                            onChange={(e) => setInputValue(e.target.value)}
                                            onBlur={() => setIsEditing(false)}
                                            className="form-control"
                                            style={{width: "70px"}}
                                            autoFocus
                                        />
                                    </form>
                                ) : (
                                    <button
                                        className="page-link"
                                        onClick={() => setIsEditing(true)}
                                    >
                                        ...
                                    </button>
                                )}
                            </li>
                        )}
                        <li className="page-item">
                            <button
                                className="page-link"
                                onClick={() => onPageChange(totalPages)}
                            >
                                {totalPages}
                            </button>
                        </li>
                    </>
                )}

                <li className={`page-item ${currentPage === totalPages ? "disabled" : ""}`}>
                    <button
                        className="page-link"
                        onClick={() => {
                            onPageChange(currentPage + 1);
                        }}
                        disabled={currentPage === totalPages}
                    >
                        Следующая
                    </button>
                </li>
            </ul>
        </nav>
    )
}

export default Pagination;