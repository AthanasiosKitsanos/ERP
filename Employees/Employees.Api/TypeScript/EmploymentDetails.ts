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

    if(formatDate(new Date(details.hireDate!)) === "01/01/1")
    {
        document.getElementById("HireDate")!.innerHTML = "";
    }
    else
    {
        if(details.hireDate)
        {
            document.getElementById("HireDate")!.innerHTML = formatDate(new Date(details.hireDate));
        }
    }
    document.getElementById("ContractType")!.innerHTML = details.contractType ?? "";
    document.getElementById("WorkLocation")!.innerHTML = details.workLocation ?? "";

    const anchotTd = document.getElementById("create-update-EmpDe") as HTMLTableCellElement;
    const anchorPath = document.createElement("a") as HTMLAnchorElement;

    const isEmpty: boolean = !details.position && !details.department && !details.employmentStatus && !details.contractType && !details.workLocation;

    anchorPath.href = "#";
    anchorPath.innerText = isEmpty ? "Add" : "Edit";

    anchorPath.addEventListener("click", async e =>
    {
        e.preventDefault();

        if(isEmpty)
        {
            await createEmploymentDetails(id, container, details);
        }
        else
        {
            await editEmploymentDetails(id, container, details);
        }
    })

    anchotTd.appendChild(anchorPath);
}

async function createEmploymentDetails(id: number, container: HTMLDivElement, details: EmploymentDetails): Promise<void>
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
            await getView(id, container, details);
            return;
        }

        details = await getEmploymentDetails(id);

        await getView(id, container, details);
    })

    await cancelEmploymentDetails(id, container);
}

async function editEmploymentDetails(id: number, container: HTMLDivElement, details: EmploymentDetails)
{
    let response = await fetch(`/employmentdetails/update`);

    container.innerHTML = await response.text();

    const position = document.getElementById("position") as HTMLInputElement;
    position.placeholder = details.position ?? "";
    
    const department = document.getElementById("department") as HTMLInputElement;
    department.placeholder = details.department ?? "";

    const employmentstatus = document.getElementById("employmentstatus") as HTMLInputElement;
    employmentstatus.placeholder = details.employmentStatus ?? "";

    const hiredate = document.getElementById("hiredate") as HTMLInputElement;
    hiredate.placeholder = formatDate(new Date(details.hireDate!)) ?? "";

    const contracttype = document.getElementById("contracttype") as HTMLInputElement;
    contracttype.placeholder = details.contractType ?? "";
    
    const worklocation = document.getElementById("worklocation") as HTMLInputElement;
    worklocation.placeholder = details.workLocation ?? "";

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
            await getView(id, container, details);
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