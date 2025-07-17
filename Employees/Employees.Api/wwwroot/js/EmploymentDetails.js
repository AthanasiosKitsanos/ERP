import { formatDate } from "./Global/FormatDate.js";
import { assignField } from "./Global/FieldUpdate.js";
async function getEmploymentDetails(id) {
    const response = await fetch(`/${id}/employmentdetails/get`);
    const details = await response.json();
    return details;
}
async function getView(id, container, details) {
    const response = await fetch(`/employmentdetails/get`);
    container.innerHTML = await response.text();
    document.getElementById("Position").innerHTML = details.position ?? "";
    document.getElementById("Department").innerHTML = details.department ?? "";
    document.getElementById("EmploymentStatus").innerHTML = details.employmentStatus ?? "";
    document.getElementById("HireDate").innerHTML = details.hireDate?.toString() === "0001-01-01" || !details.hireDate ? "" : formatDate(new Date(details.hireDate));
    document.getElementById("ContractType").innerHTML = details.contractType ?? "";
    document.getElementById("WorkLocation").innerHTML = details.workLocation ?? "";
    const anchotTd = document.getElementById("create-update-EmpDe");
    const anchorPath = document.createElement("a");
    const isEmpty = !details.position && !details.department && !details.employmentStatus && !details.contractType && !details.workLocation;
    anchorPath.href = "#";
    anchorPath.innerText = isEmpty ? "Add" : "Edit";
    anchorPath.addEventListener("click", async (e) => {
        e.preventDefault();
        if (isEmpty) {
            return await createEmploymentDetails(id, container, details);
        }
        await editEmploymentDetails(id, container, details);
    });
    anchotTd.appendChild(anchorPath);
}
let submitBound = null;
async function createEmploymentDetails(id, container, details) {
    let response = await fetch(`/employmentDetails/create`);
    container.innerHTML = await response.text();
    const submit = document.getElementById("createEmploymentDetails");
    if (!submit) {
        return;
    }
    if (submitBound) {
        submit.removeEventListener("submit", submitBound);
    }
    const newSubmitBound = (e) => submitHandler(e, id, container, details);
    submitBound = newSubmitBound;
    submit.addEventListener("submit", submitBound);
    await cancelEmploymentDetails(id, container, details);
}
async function submitHandler(e, id, container, details) {
    e.preventDefault();
    const form = e.target;
    const data = new FormData(form);
    const response = await fetch(`/${id}/employmentdetails/create`, { method: 'POST', body: data });
    document.querySelectorAll("input").forEach(input => input.placeholder = "");
    if (!response.ok && response.status === 400) {
        const errors = await response.json();
        for (const field in errors) {
            const errorMessage = errors[field];
            const errorPlaceholder = document.getElementById(field);
            if (errorPlaceholder) {
                errorPlaceholder.placeholder = errorMessage.join(", ");
            }
        }
        return;
    }
    const result = await response.json();
    if (!result.success) {
        alert("No employment details added");
        return await getView(id, container, details);
    }
    const createdData = result.data;
    Object.assign(details, createdData);
    await getView(id, container, details);
}
async function editEmploymentDetails(id, container, details) {
    let response = await fetch(`/employmentdetails/update`);
    container.innerHTML = await response.text();
    const position = document.getElementById("position");
    position.placeholder = details.position ?? "";
    const department = document.getElementById("department");
    department.placeholder = details.department ?? "";
    const employmentstatus = document.getElementById("employmentstatus");
    employmentstatus.placeholder = details.employmentStatus ?? "";
    const hiredate = document.getElementById("hiredate");
    hiredate.placeholder = formatDate(new Date(details.hireDate)) ?? "";
    const contracttype = document.getElementById("contracttype");
    contracttype.placeholder = details.contractType ?? "";
    const worklocation = document.getElementById("worklocation");
    worklocation.placeholder = details.workLocation ?? "";
    const submit = document.getElementById("editEmploymentDetails");
    if (!submit) {
        return;
    }
    submit.addEventListener("submit", async (e) => {
        e.preventDefault();
        const form = e.target;
        const data = new FormData(form);
        response = await fetch(`/${id}/employmentdetails/update`, { method: 'POST', body: data });
        const result = await response.json();
        if (!result.success) {
            alert("No details were updated");
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
    await cancelEmploymentDetails(id, container, details);
}
async function cancelEmploymentDetails(id, container, details) {
    const cancelButton = document.getElementById("cancelEmploymentDetails");
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
    const details = await getEmploymentDetails(id);
    let container = document.getElementById("employmentDetails");
    await getView(id, container, details);
});
