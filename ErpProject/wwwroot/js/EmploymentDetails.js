document.addEventListener("DOMContentLoaded", async function()
{
    await window.refreshCheckCompleted;
    
    const Id = window.Id;

    let response = await fetch(`/EmploymentDetails/${Id}/Index`);

    let html = await response.text();

    let container = document.getElementById("employmentDetailsContainer");

    container.innerHTML = html;

    EdRegistrationForm(Id);

    EdEditForm(Id);

    function EdRegistrationForm(Id)
    {
        const registerButton = document.getElementById("EdRegistrationForm");

        if(registerButton)
        {
            registerButton.addEventListener("click", async function(event)
            {
                event.preventDefault()

                response = await fetch(`/EmploymentDetails/${Id}/Register`);

                html = await response.text();

                container.innerHTML = html;

                CancelEdForm(Id);
            });
        }
    };

    function EdEditForm(Id)
    {
        const editButton = document.getElementById("EdEditForm");

        if(editButton)
        {
            editButton.addEventListener("click", async function(event)
            {
                event.preventDefault();

                response = await fetch(`/EmploymentDetails/${Id}/Edit`);

                html = await response.text();

                container.innerHTML = html;

                CancelEdForm(Id);
            });
        }
    }

    function CancelEdForm(Id)
    {
        const cancelButton = document.getElementById("CancelEdForm");

        if(cancelButton)
        {
            cancelButton.addEventListener("click", async function(event)
            {
                event.preventDefault();

                response = await fetch(`/EmploymentDetails/${Id}/Index`);

                html = await response.text();

                container.innerHTML = html;

                EdRegistrationForm(Id);

                EdEditForm(Id);
            });
        }
    }
});