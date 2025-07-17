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
async function createRole(id, container, role) {
    let response = await fetch(`/roles/create`);
    container.innerHTML = await response.text();
    const form = document.getElementById("roleForm");
    response = await fetch(`/${id}/roles/getall`);
    const roleList = await response.json();
    const td = document.getElementById("roleselection");
    const selection = document.createElement("select");
    selection.name = "Id";
    roleList.forEach((role) => {
        const option = document.createElement("option");
        Object.entries(role).forEach(([key, value]) => {
            if (key === "id") {
                option.value = value;
            }
            if (key === "roleName") {
                option.text = value;
            }
        });
        selection.appendChild(option);
    });
    td.appendChild(selection);
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
        const updatedData = result.data;
        const newRole = roleList.find((r) => r.id === updatedData.id);
        if (newRole) {
            role = newRole;
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
