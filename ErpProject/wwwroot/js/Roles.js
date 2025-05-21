document.addEventListener("DOMContentLoaded", async function()
{
    const Id = window.Id;

    let response = await fetch(`/Roles/Index/${Id}`);

    let html = await response.text();

    let container = document.getElementById("rolesContainer");

    container.innerHTML = html;

    RegisterRole(Id);
    EditRole(Id);

    function RegisterRole(Id)
    {
        const registerRole = document.getElementById("registerRole");

        if(registerRole)
        {
            registerRole.addEventListener("click", async e =>
            {
                e.preventDefault();

                response = await fetch(`/Roles/Register/${Id}`);

                html = await response.text();

                container.innerHTML = html;

                CancelRolesForm(Id);
            });

            
        }
    }

    function EditRole(Id)
    {
        const editRole = document.getElementById("editRole");

        if(editRole)
        {
            editRole.addEventListener("click", async e =>
            {
                e.preventDefault();

                response = await fetch(`/Roles/Edit/${Id}`);

                html = await response.text();

                container.innerHTML = html;
                
                CancelRolesForm(Id);
            });
        }
    }

    function CancelRolesForm(Id)
    {
        const cancelForm =  document.getElementById("cancelRoleForm");

        if(cancelForm)
        {
            cancelForm.addEventListener("click", async e =>
            {
                e.preventDefault();

                response = await fetch(`/Roles/Index/${Id}`);

                html = await response.text();

                container.innerHTML = html;

                RegisterRole(Id);
                EditRole(Id);
            });
        }
    }
});