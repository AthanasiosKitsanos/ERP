document.addEventListener("DOMContentLoaded", async function()
{
    try
    {
        const response = await fetch(`/api/refreshtoken`,
        {
            method: 'GET',
            credentials: 'include'
        });

        if(response.ok)
        {
            console.log("Access token is valid");
            return true;
        }

        if(response.status === 401)
        {
            const requestNewToken = await fetch(`/refresh-token`,
            {
                method: 'POST',
                credentials: 'include'
            });

            if(requestNewToken.ok)
            {
                console.log("Token Refreshed");
                return true;
            }
            else
            {
                console.warn("Refresh Token is invalid");
                window.location.href = '/LogIn/Index';
                return false;
            }
        }

        throw new Error("Uknown Error");
    }
    catch (error)
    {
        console.error("Token check failed", err);
        window.location.href = '/LogIn/Index';
        return false;
    }
});