import { Employee } from "./Models/Employee";
import { formatDate } from "./Global/FormatDate.js";
import { assignField } from "./Global/FieldUpdate.js";

async function cancelForm(id: number, container: HTMLDivElement, employee: Employee): Promise<void>
{
    const cancel = document.getElementById("cancelDetailsForm");

    if(!cancel)
    {
        return;
    }

    cancel.addEventListener("click", async e =>
    {
        e.preventDefault();
        await getView(id, container, employee);
    })
} 

async function getView(id: number, container: HTMLDivElement, employee: Employee):Promise<void>
{
    const response: Response = await fetch(`/employees/${id}/getmaindetails`);
    const html: string = await response.text();
    
    container.innerHTML = html;

    document.getElementById("FirstName")!.innerText = employee.firstName ?? "";
    document.getElementById("LastName")!.innerText = employee.lastName ?? "";
    document.getElementById("Email")!.innerText = employee.email ?? "";
    document.getElementById("Age")!.innerText = employee.age ?? "";

    if(employee.dateOfBirth)
    {
        document.getElementById("DateOfBirth")!.innerText = formatDate(new Date(employee.dateOfBirth));
    }
    
    document.getElementById("Nationality")!.innerText = employee.nationality ?? "";
    document.getElementById("Gender")!.innerText = employee.gender ?? "";
    document.getElementById("PhoneNumber")!.innerText = employee.phoneNumber ?? "";

    await getUpdateView(id, container, employee);
}

async function getUpdateView(id: number, container: HTMLDivElement, employee: Employee): Promise<void>
{
    const formButton =  document.getElementById("updateDetailsForm") as HTMLAnchorElement;

    if(!formButton)
    {
        return;
    }

    formButton.addEventListener("click", async e =>
    {
        e.preventDefault();

        const response: Response = await fetch(`/employees/${id}/update`);
        const html: string = await response.text();
        container.innerHTML = html;

        document.getElementById("FirstName")!.innerText = employee.firstName ?? "";
        document.getElementById("LastName")!.innerText = employee.lastName ?? "";

        const email = document.getElementById("Email") as HTMLInputElement;
        email.placeholder = employee.email ?? "";

        document.getElementById("Age")!.innerText = employee.age ?? "";

        if(employee.dateOfBirth)
        {
            document.getElementById("DateOfBirth")!.innerText = formatDate(new Date(employee.dateOfBirth));
        }

        const nationality = document.getElementById("Nationality") as HTMLInputElement;
        nationality.placeholder = employee.nationality ?? "";

        document.getElementById("Gender")!.innerText = employee.gender ?? "";

        const phonenumber = document.getElementById("PhoneNumber") as HTMLInputElement;
        phonenumber.placeholder = employee.phoneNumber ?? "";

        await submitDetails(id, container, employee);

        await cancelForm(id, container, employee);
    })   
}

async function submitDetails(id: number, container: HTMLDivElement, employee: Employee): Promise<void>
{
    const onSubmit = document.getElementById("employee-update") as HTMLFormElement;

    if(!onSubmit)
    {
        alert("There was something wrong");
        return;
    }
    
    onSubmit.addEventListener("submit", async e =>
    {
        e.preventDefault();

        const form = e.target as HTMLFormElement;
        const data = new FormData(form);

        const response: Response = await fetch(`/${id}/getmaindetails/update`, {method: 'POST', body: data})

        const result = await response.json();

        if(!result.success)
        {
            alert("Employee details were not updated");
            return await getView(id, container, employee);
        }
        
        const updatedData: Partial<Employee> = result.data;
        for(const key in updatedData)
        {
            const keyType = key as keyof typeof updatedData;
            const value = updatedData[keyType];
            if(value !== "" && value !== null && value !== undefined)
            {
                assignField(employee, keyType, value);
            }
        }
        
        await getView(id, container, employee);
        
    })
}

async function getEmployee(id: number):Promise<Employee>
{
    const response = await fetch(`/${id}/getmaindetails`);

    const employee: Employee = await response.json();

    if(!employee)
    {
        alert("No employee was found");
    }

    return employee;
}

document.addEventListener("DOMContentLoaded", async () =>
{
    const id: number = window.Id;

    const employee: Employee = await getEmployee(id);

    let container = document.getElementById("mainDetails") as HTMLDivElement;
    
    await getView(id, container, employee);
})