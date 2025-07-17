import { Role } from "./Models/Roles";

async function getRole(id: number): Promise<Role>
{
    const response: Response = await fetch(`/${id}/roles/get`);

    const role: Role = await response.json();

    return role;
}

async function getView(id: number, container: HTMLDivElement, role: Role): Promise<void>
{
    const response: Response = await fetch(`/roles/get`);

    container.innerHTML = await response.text();

    document.getElementById("RoleName")!.innerText = role.roleName ?? "";

    const anchor = document.getElementById("create-or-update-role") as HTMLAnchorElement;

    const isEmpty: boolean = !role.roleName;

    anchor.innerText = isEmpty ? "Add" : "Edit";

    anchor.addEventListener("click", async e =>
    {
        e.preventDefault();
        if(isEmpty)
        {
            return await createRole(id, container, role);
        }

        await editRole(id, container, role);
    })
}

async function createRole(id: number, container: HTMLDivElement, role: Role): Promise<void>
{
    let response: Response = await fetch(`/roles/create`);

    container.innerHTML = await response.text();

    const form = document.getElementById("roleForm") as HTMLFormElement;

    response = await fetch(`/${id}/roles/getall`);

    const roleList = await response.json();

    const td = document.getElementById("roleselection") as HTMLTableRowElement;
    const selection = document.createElement("select") as HTMLSelectElement;
    selection.name = "Id";
    
    roleList.forEach((role: Role) =>
    {
        const option = document.createElement("option") as HTMLOptionElement;

        Object.entries(role).forEach(([key, value]) =>
        {
            if(key === "id")
            {
                option.value = value;
            }

            if(key === "roleName")
            {
                option.text = value;
            }
        })

        selection.appendChild(option);
    })

    td.appendChild(selection);

}

async function editRole(id: number, container: HTMLDivElement, role: Role): Promise<void>
{
    let response: Response = await fetch(`/roles/update`);

    container.innerHTML = await response.text();

    const submit = document.getElementById("editRoleForm") as HTMLFormElement;

    response = await fetch(`/${id}/roles/getall`);

    const roleList: Role[] = await response.json();

    const selection = document.getElementById("roleSelection") as HTMLSelectElement;
    
    roleList.forEach((r: Role) =>
    {
        const option = document.createElement("option") as HTMLOptionElement;
        if(r.id)
        {
            option.value = r.id.toString();
        }
        
        if(r.roleName)
        {
            option.text = r.roleName;
        }

        selection.appendChild(option);
    })

    submit.addEventListener("submit", async e =>
    {
        e.preventDefault();

        const form = e.target as HTMLFormElement;
        const data = new FormData(form);

        const response = await fetch(`/${id}/roles/update`, { method: 'POST', body: data});

        const result = await response.json();

        if(!result.success)
        {
            alert(`Role Id = ${role.id} and Selected id = ${selection.value}`);
            return await getView(id, container, role);
        }

        const updatedData : Partial<Role> = result.data;
        const newRole = roleList.find((r: Role) => r.id === updatedData.id);

        if(newRole)
        {
            role = newRole;
        }

        await getView(id, container, role);
    })

    await cancelRoleForm(id, container, role);
}

async function cancelRoleForm(id: number, container: HTMLDivElement, role: Role): Promise<void>
{
    const cancel = document.getElementById("cancelRolesForm") as HTMLAnchorElement;

    cancel.addEventListener("click", async e =>
    {
        e.preventDefault();

        await getView(id, container, role);
    })
}

document.addEventListener("DOMContentLoaded", async e =>
{
    const id: number = window.Id;

    const role: Role = await getRole(id);

    let container = document.getElementById("roles") as HTMLDivElement;

    await getView(id, container, role);
})