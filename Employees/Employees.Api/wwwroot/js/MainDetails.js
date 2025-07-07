document.addEventListener("DOMContentLoaded", async function()
{   
    const Id = window.Id;

    let container = document.getElementById("employeeDetailsContainer");

    let response = await fetch(`/employees/${Id}/getmaindetails`);

    let html = await response.text();

    container.innerHTML = html;

    EditEmployeeButton(Id);

    function EditEmployeeButton(Id)
    {   
        const edit = document.getElementById("MDEditForm");

        if(edit)
        {
            edit.addEventListener("click", async function(event)
            {
                event.preventDefault();
                
                response = await fetch(`/employees/${Id}/update`);

                html = await response.text();

                container.innerHTML = html;
                
                ConfirmUpdate(Id);

                CancelMDForm(Id);
            });
        }
    }

    function CancelMDForm(Id)
    {
        const cancel = document.getElementById("CancelMDForm");

        if(cancel)
        {
            cancel.addEventListener("click", async e =>
            {
                e.preventDefault();

                response = await fetch(`/employees/${Id}/getmaindetails`);

                html = await response.text();

                container.innerHTML = html;

                EditEmployeeButton(Id);
            });
        }
    }

    function ConfirmUpdate(Id)
    {
        const updateForm = document.getElementById("employee-update");

        if(updateForm)
        {
            updateForm.addEventListener("submit", async e =>
            {
                e.preventDefault();

                const form = e.target;
                const data = new FormData(form);

                const response  = await fetch(`/employees/${Id}/update`, { method: 'POST', body: data});

                const result = await response.json();

                if(result.success)
                {
                    const partial = await fetch(`/employees/${Id}/getmaindetails`);
                    html = await partial.text();
                    container.innerHTML = html;

                    EditEmployeeButton(Id);
                }
                else
                {
                    alert("Not updated");
                }
            });
        }
    }
});