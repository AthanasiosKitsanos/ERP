document.addEventListener("DOMContentLoaded", async function()
{
    await window.refreshCheckCompleted;
    
    const Id = window.Id;

    const response = await fetch(`/Photo/Index/${Id}`);

    const html = await response.text();

    document.getElementById("photo").innerHTML = html;
});