document.addEventListener("DOMContentLoaded", async function ()
{
    let work = document.getElementById("work");

    const text = ['.', '..', '...'];

    let index = 0;

    dotMethod();

    setInterval(dotMethod, 500);

    function dotMethod()
    {
        if(work)
        {   
            work.innerHTML = `Work in Progress${text[index]}`;

            index = (index + 1) % text.length;
        }
    }
    
});