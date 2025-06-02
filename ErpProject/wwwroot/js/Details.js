import { checkRefreshToken } from "./CheckRefreshToken.js";

document.addEventListener("DOMContentLoaded", function()
{
    checkRefreshToken().then(valid =>
    {
        if(valid)
        {     
            const scripts = 
            [
                '/js/AdditionalDetails.js',
                '/js/EmploymentDetails.js',
                '/js/Identifications.js',
                '/js/MainDetails.js',
                '/js/Photo.js',
                '/js/Roles.js'
            ];

            for(const src of scripts)
            {
                const script = document.createElement('script');
                script.setAttribute('type', 'module');
                script.setAttribute('src', src);
                document.body.appendChild(script);
                console.log(`${src} is loaded`);
            }
        }
    });
});