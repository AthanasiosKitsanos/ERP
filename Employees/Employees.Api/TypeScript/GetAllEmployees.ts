import { Employee } from "./Models/Employee";

async function GetAllEmployees(): Promise<void>
{
    const tbody = document.getElementById('employeeTable');

    if(!tbody)
    {
        return;
    }

    const response = await fetch('/getallemployees');

    const employeeList = await response.json();

    employeeList.forEach((employee: Employee) => 
    {
        const tr = document.createElement('tr');
        
        Object.entries(employee).forEach(([key, value]) =>
        {
            if(key === "id")
            {
                return;
            }

            const valueTd = document.createElement('td');
            valueTd.innerHTML = value.toString();

            tr.appendChild(valueTd);
        })
        
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

document.addEventListener("DOMContentLoaded", async () =>
{
    await GetAllEmployees();
})