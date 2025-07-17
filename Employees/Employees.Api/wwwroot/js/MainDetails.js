import { formatDate } from "./Global/FormatDate.js";
import { assignField } from "./Global/FieldUpdate.js";
async function cancelForm(id, container, employee) {
    const cancel = document.getElementById("cancelDetailsForm");
    if (!cancel) {
        return;
    }
    cancel.addEventListener("click", async (e) => {
        e.preventDefault();
        await getView(id, container, employee);
    });
}
async function getView(id, container, employee) {
    const response = await fetch(`/employees/${id}/getmaindetails`);
    const html = await response.text();
    container.innerHTML = html;
    document.getElementById("FirstName").innerText = employee.firstName ?? "";
    document.getElementById("LastName").innerText = employee.lastName ?? "";
    document.getElementById("Email").innerText = employee.email ?? "";
    document.getElementById("Age").innerText = employee.age ?? "";
    if (employee.dateOfBirth) {
        document.getElementById("DateOfBirth").innerText = formatDate(new Date(employee.dateOfBirth));
    }
    document.getElementById("Nationality").innerText = employee.nationality ?? "";
    document.getElementById("Gender").innerText = employee.gender ?? "";
    document.getElementById("PhoneNumber").innerText = employee.phoneNumber ?? "";
    await getUpdateView(id, container, employee);
}
async function getUpdateView(id, container, employee) {
    const formButton = document.getElementById("updateDetailsForm");
    if (!formButton) {
        return;
    }
    formButton.addEventListener("click", async (e) => {
        e.preventDefault();
        const response = await fetch(`/employees/${id}/update`);
        const html = await response.text();
        container.innerHTML = html;
        document.getElementById("FirstName").innerText = employee.firstName ?? "";
        document.getElementById("LastName").innerText = employee.lastName ?? "";
        const email = document.getElementById("Email");
        email.placeholder = employee.email ?? "";
        document.getElementById("Age").innerText = employee.age ?? "";
        if (employee.dateOfBirth) {
            document.getElementById("DateOfBirth").innerText = formatDate(new Date(employee.dateOfBirth));
        }
        const nationality = document.getElementById("Nationality");
        nationality.placeholder = employee.nationality ?? "";
        document.getElementById("Gender").innerText = employee.gender ?? "";
        const phonenumber = document.getElementById("PhoneNumber");
        phonenumber.placeholder = employee.phoneNumber ?? "";
        await submitDetails(id, container, employee);
        await cancelForm(id, container, employee);
    });
}
async function submitDetails(id, container, employee) {
    const onSubmit = document.getElementById("employee-update");
    if (!onSubmit) {
        alert("There was something wrong");
        return;
    }
    onSubmit.addEventListener("submit", async (e) => {
        e.preventDefault();
        const form = e.target;
        const data = new FormData(form);
        const response = await fetch(`/${id}/getmaindetails/update`, { method: 'POST', body: data });
        const result = await response.json();
        if (!result.success) {
            alert("Employee details were not updated");
            return await getView(id, container, employee);
        }
        const updatedData = result.data;
        for (const key in updatedData) {
            const keyType = key;
            const value = updatedData[keyType];
            if (value !== "" && value !== null && value !== undefined) {
                assignField(employee, keyType, value);
            }
        }
        await getView(id, container, employee);
    });
}
async function getEmployee(id) {
    const response = await fetch(`/${id}/getmaindetails`);
    const employee = await response.json();
    if (!employee) {
        alert("No employee was found");
    }
    return employee;
}
document.addEventListener("DOMContentLoaded", async () => {
    const id = window.Id;
    const employee = await getEmployee(id);
    let container = document.getElementById("mainDetails");
    await getView(id, container, employee);
});
