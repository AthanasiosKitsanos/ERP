document.addEventListener("DOMContentLoaded", async function()
{
    const Id = window.Id;

    const response = await fetch(`/Employee/MainDetails/${Id}`);

    html = await response.text();

    document.getElementById("employeeDetails").innerHTML = html;
});