import { EmploymentDetails } from "./Models/EmploymentDetails";

function formatDate(date: Date): string
{
    const day = String(date.getDate()).padStart(2, "0"); 
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const year = date.getFullYear();

    return `${day}/${month}/${year}`;
}

async function getEmploymentDetails(id: number): Promise<EmploymentDetails>
{
    const response: Response = await fetch(`/${id}/employmentdetails/get`);

    const details: EmploymentDetails = await response.json();

    return details;
}

async function getView(id: number, container: HTMLDivElement, details: EmploymentDetails): Promise<void>
{
    const response: Response = await fetch(`/employmentdetails/get`);

    container.innerHTML = await response.text();

    document.getElementById("Position")!.innerHTML = details.position ?? "";
    document.getElementById("Department")!.innerHTML = details.department ?? "";
    document.getElementById("EmploymentStatus")!.innerHTML = details.employmentStatus ?? "";
    document.getElementById("HireDate")!.innerHTML = formatDate(new Date(details.hireDate!));
    document.getElementById("ContractType")!.innerHTML = details.contractType ?? "";
    document.getElementById("WorkLocation")!.innerHTML = details.workLocation ?? "";

    const anchotTd = document.getElementById("create-update-EmpDe") as HTMLTableCellElement;
    const anchorPath = document.createElement("a") as HTMLAnchorElement;

    const isEmpty: boolean = !details.position && !details.department && !details.employmentStatus && !details.hireDate && !details.contractType && !details.workLocation;

    anchorPath.href = "#";
    anchorPath.innerText = isEmpty ? "Add" : "Edit";

    anchorPath.addEventListener("click", async e =>
    {
        e.preventDefault();

        if(isEmpty)
        {
            await createEmploymentDetails(id, container);
        }
        else
        {
            await editEmploymentDetails(id, container, details);
        }
    })

    anchotTd.appendChild(anchorPath);
}

async function createEmploymentDetails(id: number, container: HTMLDivElement): Promise<void>
{
    let response: Response = await fetch(`/employmentDetails/create`);
    container.innerHTML = await response.text();

    const submit = document.getElementById("createEmploymentDetails") as HTMLFormElement;

    if(!submit)
    {
        return;
    }

    submit.addEventListener("submit", async e =>
    {
        e.preventDefault();

        const form = e.target as HTMLFormElement;
        const data = new FormData(form);

        response = await fetch(`/${id}/employmentdetails/create`, {method: 'POST', body: data});
        
        const result = await response.json();

        if(!result.success)
        {
            alert("No employment details added");
            return;
        }

        const details: EmploymentDetails = await getEmploymentDetails(id);

        await getView(id, container, details);
    })

    await cancelEmploymentDetails(id, container);
}

async function editEmploymentDetails(id: number, container: HTMLDivElement, details: EmploymentDetails)
{
    let response = await fetch(`/employmentdetails/update`);

    container.innerHTML = await response.text();

    const submit = document.getElementById("editEmploymentDetails") as HTMLFormElement;

    if(!submit)
    {
        return;
    }

    submit.addEventListener("submit", async e =>
    {
        e.preventDefault();

        const form = e.target as HTMLFormElement;
        const data = new FormData(form);

        response = await fetch(`/${id}/employmentdetails/update`, {method: 'POST', body: data});

        const result = await response.json();

        if(!result.success)
        {
            alert("No details were updated");
            return;
        }

        details = await getEmploymentDetails(id);

        await getView(id, container, details);
    })

    await cancelEmploymentDetails(id, container, details);
}

async function cancelEmploymentDetails(id: number, container: HTMLDivElement, details?: EmploymentDetails)
{
    const cancelButton = document.getElementById("cancelEmploymentDetails") as HTMLAnchorElement;

    if(!cancelButton)
    {
        return;
    }

    cancelButton.addEventListener("click", async e =>
    {
        e.preventDefault();

        if(!details)
        {
            details = await getEmploymentDetails(id);
        }

        await getView(id, container, details);
    })
}
document.addEventListener("DOMContentLoaded", async () =>
{
    const id: number = window.Id;

    const details: EmploymentDetails = await getEmploymentDetails(id);

    let container = document.getElementById("employmentDetails") as HTMLDivElement;

    await getView(id, container, details);
})