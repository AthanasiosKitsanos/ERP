document.addEventListener("DOMContentLoaded", async function()
{
    const Id = window.Id;

    const response = await fetch(`/Roles/Index/${Id}`);

    const html = await response.text();

    document.getElementById("role").innerHTML = html;
});