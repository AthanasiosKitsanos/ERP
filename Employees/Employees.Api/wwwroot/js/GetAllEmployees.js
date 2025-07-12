function formatDate(date) {
    const day = String(date.getDate()).padStart(2, "0");
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
}
async function GetAllEmployees() {
    const tbody = document.getElementById('employeeTable');
    if (!tbody) {
        return;
    }
    const response = await fetch('/getallemployees');
    const employeeList = await response.json();
    employeeList.forEach((employee) => {
        const tr = document.createElement('tr');
        Object.entries(employee).forEach(([key, value]) => {
            if (key === "id") {
                return;
            }
            if (key === "dateOfBirth") {
                const valueTd = document.createElement('td');
                valueTd.innerHTML = formatDate(new Date(value));
                tr.appendChild(valueTd);
            }
            const valueTd = document.createElement('td');
            valueTd.innerHTML = value.toString();
            tr.appendChild(valueTd);
        });
        const detailsTd = document.createElement('td');
        const detailsAnchor = document.createElement('a');
        detailsAnchor.href = `/employees/${employee.id}/details`;
        detailsAnchor.innerText = 'Details';
        detailsTd.appendChild(detailsAnchor);
        const deleteTd = document.createElement('td');
        const deleteAnchor = document.createElement('a');
        deleteAnchor.href = `/employees/${employee.id}/delete`;
        deleteAnchor.innerText = 'Delete';
        deleteTd.appendChild(deleteAnchor);
        tr.append(detailsTd, deleteTd);
        tbody.appendChild(tr);
    });
}
document.addEventListener("DOMContentLoaded", async () => {
    await GetAllEmployees();
});
export {};
