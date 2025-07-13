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
    const isEmplty = !details.emergencyNumbers && !details.education;
    anchorPath.id = isEmplty ? "createAdditionalDetailsForm" : "editAdditionalDetailsForm";
    anchorPath.href = "#";
    anchorPath.innerText = isEmplty ? "Add" : "Edit";
    anchorPath.addEventListener("click", async (e) => {
        e.preventDefault();
        if (isEmplty) {
            await createAdditionalDetails(id, container);
        }
        else {
            await editAdditionalDetails(id, container, details);
        }
    });
    anchorTd.appendChild(anchorPath);
}
async function createAdditionalDetails(id, container) {
    let response = await fetch(`/additionaldetails/${id}/create`);
    const html = await response.text();
    container.innerHTML = html;
    const submit = document.getElementById("createAdditionalDetails");
    if (!submit) {
        return;
    }
    submit.addEventListener("submit", async (e) => {
        e.preventDefault();
        const form = e.target;
        const data = new FormData(form);
        response = await fetch(`/${id}/additionaldetails/create`, { method: 'POST', body: data });
        const result = await response.json();
        if (!result.success) {
            alert("No additional details added");
            return;
        }
        const details = await getAdditionalDetails(id);
        await getView(id, container, details);
    });
    await cancelAdditionalDetailsForm(id, container);
}
async function editAdditionalDetails(id, container, details) {
    let response = await fetch(`/additionaldetails/${id}/update`);
    const html = await response.text();
    container.innerHTML = html;
    const submit = document.getElementById("editAdditionalDetails");
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
            await getView(id, container, details);
            alert("Additional details not updated");
            return;
        }
        details = await getAdditionalDetails(id);
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
        if (!details) {
            details = await getAdditionalDetails(id);
        }
        await getView(id, container, details);
    });
}
document.addEventListener("DOMContentLoaded", async () => {
    const id = window.Id;
    const details = await getAdditionalDetails(id);
    let container = document.getElementById("additionalDetails");
    await getView(id, container, details);
});
export {};
