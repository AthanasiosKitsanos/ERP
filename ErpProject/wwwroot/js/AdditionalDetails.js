document.addEventListener("DOMContentLoaded", async function()
{
    const Id = window.Id;

    let response = await fetch(`/additionalDetails/index/${Id}`);

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

                response = await fetch(`/additionalDetails/register/${Id}`);

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
            
                response = await fetch(`/additionalDetails/edit/${Id}`);

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

                response = await fetch(`/AdditionalDetails/Index/${Id}`);

                html = await response.text();

                container.innerHTML = html;

                RegisterFormButton(Id);
                EditFormButton(Id);
            });
        }
    }
});