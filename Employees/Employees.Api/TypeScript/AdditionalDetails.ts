import { AdditionalDetails } from "./Models/AdditionalDetails";

async function getAdditionalDetails(id: number): Promise<AdditionalDetails>
{
    const response: Response = await fetch(`/${id}/additionaldetails/get`);

    const details: AdditionalDetails = await response.json();

    return details;
}

async function getView(id: number, container: HTMLDivElement, details: AdditionalDetails): Promise<void>
{
    const response: Response = await fetch(`/additionaldetails/${id}/get`);
    const html = await response.text();

    container.innerHTML = html;

    document.getElementById("EmergencyNumber")!.innerText = details.emergencyNumbers ?? "";
    document.getElementById("Education")!.innerText = details.education ?? "";

    const anchorTd = document.getElementById("create-or-udpate") as HTMLTableCellElement;
    const anchorPath = document.createElement("a") as HTMLAnchorElement;

    const isEmpty: boolean = !details.emergencyNumbers && !details.education;
    
    anchorPath.href = "#";
    anchorPath.innerText = isEmpty ? "Add" : "Edit";

    anchorPath.addEventListener("click", async e =>
    {
        e.preventDefault();

        if(isEmpty)
        {
            await createAdditionalDetails(id, container);
        }
        else
        {
            await editAdditionalDetails(id, container, details)
        }
    })

    anchorTd.appendChild(anchorPath);
}

async function createAdditionalDetails(id: number, container: HTMLDivElement): Promise<void>
{
    let response: Response = await fetch (`/additionaldetails/${id}/create`);
    const html = await response.text();

    container.innerHTML = html;

    const submit = document.getElementById("createAdditionalDetails") as HTMLFormElement;

    if(!submit)
    {
        return;
    }

    submit.addEventListener("submit", async e =>
    {
        e.preventDefault();

        const form = e.target as HTMLFormElement;
        const data = new FormData(form);

        response = await fetch(`/${id}/additionaldetails/create`, {method: 'POST', body: data});

        const result = await response.json();

        if(!result.success)
        {
            alert("No additional details added");
            return;
        }

        const details: AdditionalDetails = await getAdditionalDetails(id);
        
        await getView(id, container, details);
    })

    await cancelAdditionalDetailsForm(id, container);
}

async function editAdditionalDetails(id: number, container: HTMLDivElement, details: AdditionalDetails): Promise<void>
{
    let response: Response = await fetch(`/additionaldetails/${id}/update`);
    const html = await response.text();
    
    container.innerHTML = html;

    const submit = document.getElementById("editAdditionalDetails") as HTMLFormElement;

    if(!submit)
    {
        return;
    }

    submit.addEventListener("submit", async e =>
    {
        e.preventDefault();

        const form = e.target as HTMLFormElement;
        const data = new FormData(form);

        response = await fetch(`/${id}/additionaldetails/update`, {method: 'POST', body: data});

        const result = await response.json();

        if(!result.success)
        {
            await getView(id, container, details);
            alert("Additional details not updated");
            return;
        }

        details = await getAdditionalDetails(id);

        await getView(id, container, details);
    })

    await cancelAdditionalDetailsForm(id, container, details);
}

async function cancelAdditionalDetailsForm(id: number, container: HTMLDivElement, details?: AdditionalDetails): Promise<void>
{
    const cancelButton = document.getElementById("cancelAdditionalDetails") as HTMLAnchorElement;

    if(!cancelButton)
    {
        return;
    }

    cancelButton.addEventListener("click", async e =>
    {
        e.preventDefault();

        if(!details)
        {
            details = await getAdditionalDetails(id);
        }

        await getView(id, container, details);
    })
}


document.addEventListener("DOMContentLoaded", async ()=>
{
    const id: number = window.Id;

    const details: AdditionalDetails = await getAdditionalDetails(id);

    let container = document.getElementById("additionalDetails") as HTMLDivElement;

    await getView(id, container, details);
})