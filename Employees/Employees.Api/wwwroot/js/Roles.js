async function getRole(id) {
    const response = await fetch(`/${id}/roles/get`);
    const role = await response.json();
    return role;
}
async function getView(id, container, role) {
    const response = await fetch(`/roles/get`);
    container.innerHTML = await response.text();
    document.getElementById("RoleName").innerText = role.roleName ?? "";
    const anchor = document.getElementById("create-or-update-role");
    const isEmpty = !role.roleName;
    anchor.innerText = isEmpty ? "Add" : "Edit";
    anchor.addEventListener("click", async (e) => {
        e.preventDefault();
        if (isEmpty) {
            return await createRole(id, container, role);
        }
        await editRole(id, container, role);
    });
}
let submitBound = null;
async function createRole(id, container, role) {
    let response = await fetch(`/roles/create`);
    container.innerHTML = await response.text();
    const submit = document.getElementById("createRoleForm");
    response = await fetch(`/${id}/roles/getall`);
    const roleList = await response.json();
    const selection = document.getElementById("roleSelection");
    roleList.forEach((role) => {
        const option = document.createElement("option");
        if (role.id) {
            option.value = role.id.toString();
        }
        if (role.roleName) {
            option.text = role.roleName;
        }
        selection.appendChild(option);
    });
    const selectedOption = selection.options[selection.selectedIndex];
    if (submitBound) {
        submit.removeEventListener("submit", submitBound);
    }
    const newSubmitBound = (e) => handleSubmit(e, id, container, role, selection);
    submitBound = newSubmitBound;
    submit.addEventListener("submit", submitBound);
    await cancelRoleForm(id, container, role);
}
async function handleSubmit(e, id, container, role, selection) {
    e.preventDefault();
    const form = e.target;
    const data = new FormData(form);
    const response = await fetch(`/${id}/roles/create`, { method: 'POST', body: data });
    if (!response.ok && response.status === 400) {
        const errors = await response.json();
        const defaultOption = document.getElementById("defaultOption");
        const defaultText = defaultOption.text;
        for (const field in errors) {
            defaultOption.text = errors[field].join(", ");
        }
        setTimeout(() => { defaultOption.text = defaultText; }, 1750);
        return;
    }
    const result = await response.json();
    if (!result.success) {
        return await getView(id, container, role);
    }
    const selectedOption = selection.options[selection.selectedIndex];
    if (selectedOption) {
        role.id = parseInt(selectedOption.value);
        role.roleName = selectedOption.text;
        console.log(`Role Id = ${role.id} and RoleName = ${role.roleName}`);
    }
    await getView(id, container, role);
}
async function editRole(id, container, role) {
    let response = await fetch(`/roles/update`);
    container.innerHTML = await response.text();
    const submit = document.getElementById("editRoleForm");
    response = await fetch(`/${id}/roles/getall`);
    const roleList = await response.json();
    const selection = document.getElementById("roleSelection");
    roleList.forEach((r) => {
        const option = document.createElement("option");
        if (r.id) {
            option.value = r.id.toString();
        }
        if (r.roleName) {
            option.text = r.roleName;
        }
        selection.appendChild(option);
    });
    submit.addEventListener("submit", async (e) => {
        e.preventDefault();
        const form = e.target;
        const data = new FormData(form);
        const response = await fetch(`/${id}/roles/update`, { method: 'POST', body: data });
        const result = await response.json();
        if (!result.success) {
            alert(`Role Id = ${role.id} and Selected id = ${selection.value}`);
            return await getView(id, container, role);
        }
        const selectedOption = selection.options[selection.selectedIndex];
        if (selectedOption) {
            role.id = parseInt(selectedOption.value);
            role.roleName = selectedOption.text;
        }
        await getView(id, container, role);
    });
    await cancelRoleForm(id, container, role);
}
async function cancelRoleForm(id, container, role) {
    const cancel = document.getElementById("cancelRolesForm");
    cancel.addEventListener("click", async (e) => {
        e.preventDefault();
        await getView(id, container, role);
    });
}
document.addEventListener("DOMContentLoaded", async (e) => {
    const id = window.Id;
    const role = await getRole(id);
    let container = document.getElementById("roles");
    await getView(id, container, role);
});
export {};
