document.addEventListener("DOMContentLoaded", async function()
{   
    const tbody = document.getElementById('employeeTable');

        fetch('/employees/getallemployees')
        .then(response => response.json())
        .then(data => {
            data.forEach(e => {
                
                const tr = document.createElement("tr");

                tr.innerHTML = `<td>${e.firstName}</td>
                                <td>${e.lastName}</td>
                                <td>${e.email}</td>
                                <td>${e.age}</td>
                                <td>${e.dateOfBirth}</td>
                                <td>${e.nationality}</td>
                                <td>${e.gender}</td>
                                <td>${e.phoneNumber}</td>
                                <td><a href="/employees/${e.id}/details">Details</a></td>
                                <td><a href="/employees/${e.id}/delete">Delete</a></td>`;
                
                tbody.appendChild(tr);
            });
        })
        .catch(error => console.error('Fetch error:', error));
});