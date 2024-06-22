interface PaginationProps {
    totalRecords: number;
    recordsPerPage: number;
    currentPage: number;
    onPageChange: (page: number) => void;
    onRecordsPerPageChange: (recordsPerPage: number) => void;
}

const Pagination = ({ totalRecords, recordsPerPage, currentPage, onPageChange, onRecordsPerPageChange }: PaginationProps) => {
    const totalPages = Math.ceil(totalRecords / recordsPerPage);

    return (
        <div className="flex justify-between items-center mt-4">
            <div>
                <label>
                    Records per page:
                    <select
                        value={recordsPerPage}
                        onChange={(e) => onRecordsPerPageChange(Number(e.target.value))}
                        className="ml-2 border p-2 rounded"
                        >
                        <option value={5}>5</option>
                        <option value={10}>10</option>
                        <option value={25}>25</option>
                        <option value={50}>50</option>
                        <option value={100}>100</option>
                    </select>
                </label>
            </div>
            <div>
                <button
                    onClick={() => onPageChange(currentPage - 1)}
                    disabled={currentPage === 1}
                    className="mr-2"
                >
                    Previous
                </button>
                <span>Page {currentPage} of {totalPages}</span>
                <button
                    onClick={() => onPageChange(currentPage + 1)}
                    disabled={currentPage === totalPages}
                    className="ml-2"
                >
                    Next
                </button>
            </div>
        </div>
    );
};

export default Pagination;
