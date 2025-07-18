import { assignField } from "./Global/FieldUpdate.js";
async function getAdditionalDetails(id) {
    const response = await fetch(`/${id}/additionaldetails/get`);
    const details = await response.json();
    return details;
}
async function getView(id, container, details) {
    const response = await fetch(`/additionaldetails/${id}/get`);
    const html = await response.text();
    container.innerHTML = html;
    document.getElementById("EmergencyNumber").innerText = details.emergencyNumbers ?? "";
    document.getElementById("Education").innerText = details.education ?? "";
    const anchorTd = document.getElementById("create-or-udpate");
    const anchorPath = document.createElement("a");
    const isEmpty = !details.emergencyNumbers && !details.education;
    anchorPath.href = "#";
    anchorPath.innerText = isEmpty ? "Add" : "Edit";
    anchorPath.addEventListener("click", async (e) => {
        e.preventDefault();
        if (isEmpty) {
            return await createAdditionalDetails(id, container, details);
        }
        await editAdditionalDetails(id, container, details);
    });
    anchorTd.appendChild(anchorPath);
}
let submitBound = null;
async function createAdditionalDetails(id, container, details) {
    let response = await fetch(`/additionaldetails/${id}/create`);
    container.innerHTML = await response.text();
    const submit = document.getElementById("createAdditionalDetails");
    if (!submit) {
        return;
    }
    if (submitBound) {
        submit.removeEventListener("submit", submitBound);
    }
    const newSubmitBound = (e) => handleSubmit(e, id, container, details);
    submitBound = newSubmitBound;
    submit.addEventListener("submit", submitBound);
    await cancelAdditionalDetailsForm(id, container, details);
}
async function handleSubmit(e, id, container, details) {
    e.preventDefault();
    const form = e.target;
    const data = new FormData(form);
    const response = await fetch(`/${id}/additionaldetails/create`, { method: 'POST', body: data });
    const clear = document.querySelectorAll("input").forEach(input => input.placeholder = "");
    if (!response.ok && response.status === 400) {
        const errors = await response.json();
        for (const field in errors) {
            const input = document.getElementById(field);
            if (input) {
                input.placeholder = errors[field].join(", ");
            }
        }
        setTimeout(() => {
            for (const field in errors) {
                const input = document.getElementById(field);
                input.placeholder = "";
            }
        }, 1750);
        return;
    }
    const result = await response.json();
    if (!result.success) {
        alert("No additional details added");
        return await getView(id, container, details);
    }
    const createdData = result.data;
    Object.assign(details, createdData);
    await getView(id, container, details);
}
async function editAdditionalDetails(id, container, details) {
    let response = await fetch(`/additionaldetails/${id}/update`);
    const html = await response.text();
    container.innerHTML = html;
    const submit = document.getElementById("editAdditionalDetails");
    const emergencynumbers = document.getElementById("EmergencyNumbers");
    emergencynumbers.placeholder = details.emergencyNumbers ?? "";
    const education = document.getElementById("Education");
    education.placeholder = details.education ?? "";
    if (!submit) {
        return;
    }
    submit.addEventListener("submit", async (e) => {
        e.preventDefault();
        const form = e.target;
        const data = new FormData(form);
        response = await fetch(`/${id}/additionaldetails/update`, { method: 'POST', body: data });
        const result = await response.json();
        if (!result.success) {
            alert("Additional details not updated");
            return await getView(id, container, details);
        }
        const updatedData = result.data;
        for (const key in updatedData) {
            const keyType = key;
            const value = updatedData[keyType];
            if (value !== "" && value !== null && value !== undefined) {
                assignField(details, keyType, value);
            }
        }
        await getView(id, container, details);
    });
    await cancelAdditionalDetailsForm(id, container, details);
}
async function cancelAdditionalDetailsForm(id, container, details) {
    const cancelButton = document.getElementById("cancelAdditionalDetails");
    if (!cancelButton) {
        return;
    }
    cancelButton.addEventListener("click", async (e) => {
        e.preventDefault();
        await getView(id, container, details);
    });
}
document.addEventListener("DOMContentLoaded", async () => {
    const id = window.Id;
    const details = await getAdditionalDetails(id);
    let container = document.getElementById("additionalDetails");
    await getView(id, container, details);
});
