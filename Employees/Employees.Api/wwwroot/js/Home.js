document.addEventListener("DOMContentLoaded", async function ()
{
    let work = document.getElementById("work");

    const text = ['.', '..', '...'];

    let index = 0;

    dotMethod();

    function dotMethod()
    {
        if(work)
        {   
            work.innerHTML = `Work in Progress${text[index]}`;

            index = (index + 1) % text.length;

            setTimeout(dotMethod, 500);
        }
    }
    
});