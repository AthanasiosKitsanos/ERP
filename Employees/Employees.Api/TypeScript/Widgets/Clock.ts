function updateClock(): void
{
    const dateNow = new Date();

    const hours = String(dateNow.getHours()).padStart(2, '0');
    const minutes = String(dateNow.getMinutes()).padStart(2, '0');
    const seconds = String(dateNow.getSeconds()).padStart(2, '0');

    const day = dateNow.getDate();
    const month = dateNow.getMonth();
    const year = dateNow.getFullYear();

    const time = `${hours}:${minutes}:${seconds}`;
    const date = `${day}/${month}/${year}`;

    const clock = document.getElementById("clock");

    if(clock)
    {
        clock.textContent = `${time} - ${date}`;
    }
}

document.addEventListener("DOMContentLoaded", () =>
{
    updateClock();

    setInterval(updateClock, 1000)
})