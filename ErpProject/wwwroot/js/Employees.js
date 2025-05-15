document.addEventListener("DOMContentLoaded", async function()
{
    const tbody = document.getElementById('employeeTable');

        fetch('/Employee/GetAllEmployees')
        .then(response => response.json())
        .then(data => {
            data.forEach(e => {
                const row = `<tr>
                                <td>${e.firstName}</td>
                                <td>${e.lastName}</td>
                                <td>${e.email}</td>
                                <td>${e.age}</td>
                                <td>${e.dateOfBirth}</td>
                                <td>${e.nationality}</td>
                                <td>${e.gender}</td>
                                <td>${e.phoneNumber}</td>
                                <td><a href="/employee/details/${e.id}">Details</a></td>
                                <td><a href="/employee/delete/${e.id}">Delete</a></td>
                            </tr>`;
                tbody.innerHTML += row;
            });
        })
        .catch(error => console.error('Fetch error:', error));
});