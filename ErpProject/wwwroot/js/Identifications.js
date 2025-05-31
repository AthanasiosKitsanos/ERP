document.addEventListener("DOMContentLoaded", async function()
{
    const Id = window.Id;

    let response = await fetch(`/Identifications/${Id}/Index`);

    let html = await response.text();

    let container =  document.getElementById("identificationsDetailsContainer");

    container.innerHTML = html;
    
    IdRegistrationForm(Id);

    IdEditForm(Id);

    function IdRegistrationForm(Id)
    {
        const registerButton = document.getElementById("IdRegistrationForm");

        if(registerButton)
        {
            registerButton.addEventListener("click", async function(event)
            {
                event.preventDefault();

                response = await fetch(`/Identifications/${Id}/Register`);

                html = await response.text();

                container.innerHTML = html;

                CancelIdForm(Id);
            });
        }
    }

    function IdEditForm(Id)
    {
        const editButton = document.getElementById("IdEditForm");

        if(editButton)
        {
            editButton.addEventListener("click", async function(event)
            {
                event.preventDefault();

                response = await fetch(`/Identifications/${Id}/Edit`);

                html = await response.text();

                container.innerHTML = html;

                CancelIdForm(Id);
            });
        }
    }

    function CancelIdForm(Id)
    {
        const cancelButton = document.getElementById("CancelIdForm")

        if(cancelButton)
        {
            cancelButton.addEventListener("click", async function()
            {
                response = await fetch(`/Identifications/${Id}/Index`);

                html = await response.text();

                container =  document.getElementById("identificationsDetailsContainer");

                container.innerHTML = html;
    
                IdRegistrationForm(Id);

                IdEditForm(Id);
            });
        }
    }
});