document.addEventListener("DOMContentLoaded", async function()
{
    const Id = window.Id;

    const response = await fetch(`/Photo/Index/${Id}`);

    const html = await response.text();

    document.getElementById("photo").innerHTML = html;
});