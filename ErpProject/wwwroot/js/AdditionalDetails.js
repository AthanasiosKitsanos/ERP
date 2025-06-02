document.addEventListener("DOMContentLoaded", async function()
{
    await window.refreshCheckCompleted;
    
    const Id = window.Id;

    let response = await fetch(`/additionalDetails/${Id}/index`);

    let html = await response.text();

    let container = document.getElementById("additionalDetailsContainer");

    container.innerHTML = html;

    RegisterFormButton(Id);
    
    EditFormButton(Id);

    function RegisterFormButton(Id)
    {
        const addForm = document.getElementById("AdRegistrationForm");

        if(addForm)
        {
            addForm.addEventListener("click", async function(event)
            {
                event.preventDefault();

                response = await fetch(`/additionalDetails/${Id}/register`);

                html = await response.text();

                container.innerHTML = html;

                CancelFormButton(Id);
            });
        }
    }

    function EditFormButton(Id)
    {
        const editForm = document.getElementById("AdEditForm");

        if(editForm)
        {
            editForm.addEventListener("click", async function(event)
            {
                event.preventDefault();
            
                response = await fetch(`/additionalDetails/${Id}/edit`);

                html = await response.text();

                container.innerHTML = html;

                CancelFormButton(Id);
            });
        }
    }

    function CancelFormButton(Id)
    {
        const cancelForm = document.getElementById("CancelAdForm");

        if(cancelForm)
        {
            cancelForm.addEventListener("click", async function(event)
            {
                event.preventDefault();

                response = await fetch(`/AdditionalDetails/${Id}/Index`);

                html = await response.text();

                container.innerHTML = html;

                RegisterFormButton(Id);
                EditFormButton(Id);
            });
        }
    }
});