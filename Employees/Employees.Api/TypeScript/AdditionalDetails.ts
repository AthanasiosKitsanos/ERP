import { AdditionalDetails } from "./Models/AdditionalDetails";
import { assignField } from "./Global/FieldUpdate.js";

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
            return await createAdditionalDetails(id, container, details);
        }
        
        await editAdditionalDetails(id, container, details)
        
    }   )

    anchorTd.appendChild(anchorPath);
}

let submitBound:((e: Event) => void) | null = null;

async function createAdditionalDetails(id: number, container: HTMLDivElement, details: AdditionalDetails): Promise<void>
{
    let response: Response = await fetch (`/additionaldetails/${id}/create`);

    container.innerHTML = await response.text();

    const submit = document.getElementById("createAdditionalDetails") as HTMLFormElement;

    if(!submit)
    {
        return;
    }

    if(submitBound)
    {
        submit.removeEventListener("submit", submitBound);
    }

    const newSubmitBound = (e: Event) => handleSubmit(e, id, container, details);
    submitBound = newSubmitBound;

    submit.addEventListener("submit", submitBound);

    await cancelAdditionalDetailsForm(id, container, details);
}

async function handleSubmit(e: Event, id: number, container: HTMLDivElement, details: AdditionalDetails): Promise<void>
{
    e.preventDefault();

    const form = e.target as HTMLFormElement;
    const data = new FormData(form);

    const response = await fetch(`/${id}/additionaldetails/create`, {method: 'POST', body: data});

    document.querySelectorAll("input").forEach(input => input.placeholder = "");
    
    if(!response.ok && response.status === 400)
    {
        const errors = await response.json();

        for(const field in errors)
        {
            const errorMessage = errors[field];

            const errorPlaceholder = document.getElementById(field) as HTMLInputElement;

            if(errorPlaceholder)
            {
                errorPlaceholder.placeholder = errorMessage.join(", ");
            }
        }
        
        return;
    }

    const result = await response.json();

    if(!result.success)
    {
        alert("No additional details added");
        return await getView(id, container, details);
    }

    const createdData: Partial<AdditionalDetails> = result.data;
    Object.assign(details, createdData);
    
    await getView(id, container, details);
}

async function editAdditionalDetails(id: number, container: HTMLDivElement, details: AdditionalDetails): Promise<void>
{
    let response: Response = await fetch(`/additionaldetails/${id}/update`);
    const html = await response.text();
    
    container.innerHTML = html;

    const submit = document.getElementById("editAdditionalDetails") as HTMLFormElement;

    const emergencynumbers =  document.getElementById("EmergencyNumbers") as HTMLInputElement;
    emergencynumbers.placeholder = details.emergencyNumbers ?? "";

    const education = document.getElementById("Education") as HTMLInputElement;
    education.placeholder = details.education ?? "";

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
            alert("Additional details not updated");
            return await getView(id, container, details);
        }

        const updatedData: Partial<AdditionalDetails> = result.data;
        for(const key in updatedData)
        {
            const keyType = key as keyof typeof updatedData;
            const value = updatedData[keyType];

            if(value !== "" && value !== null && value !== undefined)
            {
                assignField(details, keyType, value);
            }
        }

        await getView(id, container, details);
    })

    await cancelAdditionalDetailsForm(id, container, details);
}

async function cancelAdditionalDetailsForm(id: number, container: HTMLDivElement, details: AdditionalDetails): Promise<void>
{
    const cancelButton = document.getElementById("cancelAdditionalDetails") as HTMLAnchorElement;

    if(!cancelButton)
    {
        return;
    }

    cancelButton.addEventListener("click", async e =>
    {
        e.preventDefault();

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