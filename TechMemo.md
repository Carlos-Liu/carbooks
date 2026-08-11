# Technical Memo

### - How to connect db via pgAdmin?
1. From Aspire Dashboard, find the resource _carbooks-postgres_
2. In the ```Connection string``` part, find the values for the ```database```, ```host```, ```port```, ```username``` and ```password```.

### - How to reset the initial database?
1. Delete all the files under ```CarBooks.Database.Ef\Migrations```
2. Launch powershell and run below scripts
```bash
 C:\Repos\carbooks\src\WebAPI> dotnet ef migrations add InitialCreate --project CarBooks.Database.Ef
```