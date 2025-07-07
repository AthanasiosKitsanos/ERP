document.addEventListener("DOMContentLoaded", () =>
{
    updateClock();

    setInterval(updateClock, 1000);

    function updateClock()
    {
        const now = new Date();

        const hours = String(now.getHours()).padStart(2, '0');
        const minutes = String(now.getMinutes()).padStart(2, '0');
        const seconds = String(now.getSeconds()).padStart(2, '0');

        const day = now.getDate();
        const month = now.getMonth() + 1;
        const year = now.getFullYear();

        const timeString = `${hours}:${minutes}:${seconds}`;
        const dateString = `${day}/${month}/${year}`;

        document.getElementById("clock").textContent = `${timeString} - ${dateString}`;
    }
});