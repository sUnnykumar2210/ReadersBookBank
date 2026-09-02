# ASP.NET Core MVC + CRUD + Service Layer + Repository Pattern
## Readers Book Bank — 20-Step Exam Blueprint (VS Code + .NET 9 + SQL Server)

> Based on the Readers Book Bank sample question and the exact workflow practiced in this chat.

## 1. Read and break down the question
Identify the entity, properties, primary key/Identity, validations, required operations, MVC actions, and architecture.

For Readers Book Bank:
- Create: Add Book
- Read: View All Books
- Delete: Remove Book
- Update: **Not required**

The question requires Presentation Layer + Service Layer + Data Access Layer, EF Core, SQL Server, Data Annotations, and exception handling.

```text
Question
   |
   +--> Entity / DB fields
   +--> Required operations
   +--> MVC actions
   +--> Architecture
```

## 2. Create the MVC project

```powershell
dotnet new mvc -n ReadersBookBank
cd ReadersBookBank
code .
```

Why: the question asks for an ASP.NET MVC web application.

Check:

```powershell
dotnet --version
dotnet run
```

---

## 3. Install EF Core packages

This project targets **.NET 9**, so the versions used here are **9.0.11**.

```powershell
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.11
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.11
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.11
```

Purpose:

```text
EFCore
  -> main EF Core functionality

SqlServer
  -> EF Core <-> SQL Server

Tools
  -> EF Core tooling
```

---

## 4. Create folders

Create:

```text
Data
Repository
Services
```

Structure:

```text
Controllers -> Presentation
Models      -> MVC/UI models
Views       -> UI
Data        -> Entity + DbContext
Repository  -> Data Access Layer
Services    -> Service Layer
```

---

## 5. Create `BookDetail` entity

File:

```text
Data/BookDetail.cs
```

```csharp
namespace ReadersBookBank.Data
{
    public class BookDetail
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string Genre { get; set; }
        public bool AvailabilityStatus { get; set; }
    }
}
```

Why: this is the database entity.

```text
BookDetail
   |
   v
EF Core
   |
   v
BookDetails table
```

Question's DB fields:
- Id -> int, PK, Identity
- BookName -> string, length 35, Required
- Genre -> string, length 35, Required
- AvailabilityStatus -> bool

---

## 6. Create `BookDbContext`

File:

```text
Data/BookDbContext.cs
```

```csharp
using Microsoft.EntityFrameworkCore;

namespace ReadersBookBank.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options)
            : base(options)
        {
        }

        public DbSet<BookDetail> BookDetails { get; set; }
    }
}
```

Why:
- `DbContext` = bridge between app and database
- `DbSet<BookDetail>` = table representation for EF Core

```text
Application
    |
BookDbContext
    |
EF Core
    |
SQL Server
```

---

## 7. Add connection string

In `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=LAPTOP-KT48F59S\\SQLEXPRESS;Database=ReadersBookBankDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },

  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },

  "AllowedHosts": "*"
}
```

Meaning:

```text
Server      -> SQL Server instance
Database    -> database name
Trusted_Connection -> Windows authentication
TrustServerCertificate -> trust local certificate
```

---

## 8. Register `BookDbContext`

In `Program.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using ReadersBookBank.Data;
```

Then:

```csharp
builder.Services.AddDbContext<BookDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
```

Flow:

```text
appsettings.json
      |
      v
Program.cs
      |
      v
BookDbContext
      |
      v
SQL Server
```

---

## 9. Create `IRepository`

File:

```text
Repository/IRepository.cs
```

```csharp
using ReadersBookBank.Data;

namespace ReadersBookBank.Repository
{
    public interface IRepository
    {
        bool AddBook(BookDetail book);
        List<BookDetail> ViewAllBooks();
        bool RemoveBook(int bookId);
    }
}
```

Why: the question explicitly asks for these repository methods.

Remember:

```text
IRepository -> WHAT
Repository  -> HOW
```

---

## 10. Create Repository

File:

```text
Repository/Repository.cs
```

```csharp
using ReadersBookBank.Data;

namespace ReadersBookBank.Repository
{
    public class Repository : IRepository
    {
        private readonly BookDbContext _context;

        public Repository(BookDbContext context)
        {
            _context = context;
        }

        public bool AddBook(BookDetail book)
        {
            try
            {
                _context.BookDetails.Add(book);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<BookDetail> ViewAllBooks()
        {
            try
            {
                return _context.BookDetails.ToList();
            }
            catch
            {
                return new List<BookDetail>();
            }
        }

        public bool RemoveBook(int bookId)
        {
            try
            {
                var book = _context.BookDetails.Find(bookId);

                if (book == null)
                    return false;

                _context.BookDetails.Remove(book);
                _context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
```

Why: this is where actual EF Core database operations happen.

```text
Add    -> Add() + SaveChanges()
Read   -> ToList()
Delete -> Find() + Remove() + SaveChanges()
```

`try-catch` is included because the question asks for structured exception handling.

---

## 11. Create `IService`

File:

```text
Services/IService.cs
```

```csharp
using ReadersBookBank.Data;

namespace ReadersBookBank.Services
{
    public interface IService
    {
        bool AddBook(BookDetail book);
        List<BookDetail> ViewAllBooks();
        bool RemoveBook(int bookId);
    }
}
```

Why: Service Layer is explicitly required.

---

## 12. Create `BookService`

File:

```text
Services/BookService.cs
```

```csharp
using ReadersBookBank.Data;
using ReadersBookBank.Repository;

namespace ReadersBookBank.Services
{
    public class BookService : IService
    {
        private readonly IRepository _repository;

        public BookService(IRepository repository)
        {
            _repository = repository;
        }

        public bool AddBook(BookDetail book)
        {
            return _repository.AddBook(book);
        }

        public List<BookDetail> ViewAllBooks()
        {
            return _repository.ViewAllBooks();
        }

        public bool RemoveBook(int bookId)
        {
            return _repository.RemoveBook(bookId);
        }
    }
}
```

Correct architecture:

```text
Controller
    |
    v
Service
    |
    v
Repository
    |
    v
DbContext
    |
    v
SQL Server
```

---

## 13. Create MVC `Book` model

File:

```text
Models/Book.cs
```

```csharp
using System.ComponentModel.DataAnnotations;

namespace ReadersBookBank.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string BookName { get; set; }

        [Required]
        [StringLength(20)]
        public required string Genre { get; set; }

        public bool AvailabilityStatus { get; set; }
    }
}
```

Why two classes?

```text
BookDetail -> database entity
Book       -> MVC model / validation
```

The question explicitly asks for MVC `Book` and Data Annotations.

---

## 14. Create Controller

File:

```text
Controllers/BookController.cs
```

Main responsibility:

```text
Browser request
      |
      v
Controller
      |
      v
Service
```

Required actions for this question:
- `Index()`
- `AddBook()` GET
- `AddBook(Book)` POST
- `RemoveBook(int id)`

The Controller should not directly call the Repository when the question requires Service Layer.

---

## 15. Install scaffolding tool

One-time global tool for this .NET 9 setup:

```powershell
dotnet tool install -g dotnet-aspnet-codegenerator --version 9.0.11
```

This is global, so normally install it once per computer.

For every project where scaffolding is needed:

```powershell
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 9.0.11
```

Difference:

```text
dotnet-aspnet-codegenerator
    -> global tool
    -> one-time per machine

Web.CodeGeneration.Design
    -> project package
    -> install per project
```

---

## 16. Scaffold Views

Because Service + Repository are being handled manually, we can use scaffolding to save time on UI generation.

Index:

```powershell
dotnet aspnet-codegenerator view Index List -m Book -outDir Views/Book --useDefaultLayout
```

AddBook:

```powershell
dotnet aspnet-codegenerator view AddBook Create -m Book -outDir Views/Book --useDefaultLayout
```

Why only Views?

A fully scaffolded EF controller can go directly to `DbContext`, which can bypass Service/Repository.

```text
Avoid for this question:
Controller -> DbContext

Use:
Controller -> Service -> Repository -> DbContext
```

---

## 17. Customize scaffolded Views

Scaffolding generates a generic template, so remove actions not required by the question.

For Readers Book Bank:

```text
Create -> YES
Read   -> YES
Delete -> YES
Update -> NO
Details-> NO
```

Clean `Index.cshtml`:

```cshtml
@model IEnumerable<ReadersBookBank.Models.Book>

@{
    ViewData["Title"] = "Books";
}

<h1>Readers Book Bank</h1>

<p>
    <a asp-action="AddBook" class="btn btn-primary">Add New Book</a>
</p>

<table class="table">
    <thead>
        <tr>
            <th>Id</th>
            <th>Book Name</th>
            <th>Genre</th>
            <th>Availability Status</th>
            <th>Action</th>
        </tr>
    </thead>

    <tbody>
        @foreach (var book in Model)
        {
            <tr>
                <td>@book.Id</td>
                <td>@book.BookName</td>
                <td>@book.Genre</td>
                <td>@book.AvailabilityStatus</td>
                <td>
                    <a asp-action="RemoveBook"
                       asp-route-id="@book.Id"
                       class="btn btn-danger">
                        Remove
                    </a>
                </td>
            </tr>
        }
    </tbody>
</table>
```

Important: do not ask the user for `Id` on Add because SQL Server generates the Identity value.

---

## 18. Register Dependency Injection

In `Program.cs` add:

```csharp
using ReadersBookBank.Repository;
using ReadersBookBank.Services;
```

Then:

```csharp
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IService, BookService>();
```

Meaning:

```text
IRepository -> Repository
IService    -> BookService
```

This lets .NET inject the correct implementation into the Controller/Service.

---

## 19. Install EF CLI and create/apply migration

One-time global EF CLI tool:

```powershell
dotnet tool install --global dotnet-ef --version 9.0.11
```

Create migration:

```powershell
dotnet ef migrations add InitialCreate
```

Apply migration:

```powershell
dotnet ef database update
```

Code First flow:

```text
C# Entity
   |
   v
DbContext
   |
   v
Migration
   |
   v
database update
   |
   v
SQL Server Database
   |
   v
BookDetails table
```

---

## 20. Verify SSMS and test the application

### SSMS connection used in this project

```text
Server type: Database Engine
Server name: LAPTOP-KT48F59S\SQLEXPRESS
Authentication: Windows Authentication
```

Local certificate issue was solved by enabling:

```text
Trust Server Certificate
```

### Expected SSMS structure

```text
Databases
   |
   v
ReadersBookBankDB
   |
   v
Tables
   |
   +--> dbo.BookDetails
   +--> dbo.__EFMigrationsHistory
```

### Run application

```powershell
dotnet run
```

Then:

```text
http://localhost:5275/Book
```

### Read flow

```text
/Book
  |
  v
Index()
  |
  v
Service
  |
  v
Repository
  |
  v
SQL Server
```

### Add flow

```text
AddBook page
  |
  v
POST AddBook(Book)
  |
  v
Service
  |
  v
Repository
  |
  v
SQL Server
```

### Delete flow

```text
Remove
  |
  v
RemoveBook(id)
  |
  v
Service
  |
  v
Repository
  |
  v
SQL Server
```

---

# Master Architecture

```text
                         USER / ADMIN
                              |
                              v
                           VIEW
                              |
                              v
                         CONTROLLER
                              |
                              v
                       SERVICE LAYER
                              |
                              v
                       REPOSITORY
                              |
                              v
                        DB CONTEXT
                              |
                              v
                           EF CORE
                              |
                              v
                         SQL SERVER
                              |
                              v
                            SSMS
```

# Question -> Code Mapping

| Question says | Build |
|---|---|
| Add entity/model class | Entity |
| DbContext | DbContext |
| DbSet | Table representation |
| IRepository | Repository interface |
| Repository | DB implementation |
| Service Layer | IService + Service |
| MVC model | UI/model class |
| Data Annotations | Validation |
| GET Add/Create | Show form |
| POST Add/Create | Save |
| View All | Read |
| Edit | Update |
| Remove/Delete | Delete |
| Migration | Create/update DB schema |
| SQL Server | Actual database |
| SSMS | Verify/manage DB |

# Scaffolding Rule

## CRUD-only question

You can save time with:

```text
Scaffold Controller + Views
```

## Service Layer / Repository required

Use:

```text
Entity       -> Manual
DbContext    -> Manual
Repository   -> Manual
Service      -> Manual
Controller   -> Manual/controlled
Views        -> Scaffold
```

# IntelliSense shortcuts

```text
Ctrl + Space
    -> open suggestions

Enter / Tab
    -> accept suggestion

Ctrl + .
    -> Quick Fix / add missing using
```

# Common mistakes to avoid

1. **Do not mix EF versions randomly.**
   For this .NET 9 project use EF Core **9.0.11** consistently.

2. **Interface and implementation signatures must match.**
   Example:
   ```csharp
   bool AddBook(BookDetail book);
   ```
   must be implemented as:
   ```csharp
   public bool AddBook(BookDetail book)
   ```

3. **Do not manually create the table in SSMS** for this Code First workflow.
   Use:
   ```powershell
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. **Do not let the final Controller bypass the Service Layer.**

5. **Do not add functionality the question did not ask for.**

# One-line exam memory

```text
Question
 -> Entity
 -> DbContext
 -> Repository
 -> Service
 -> MVC Model
 -> Controller
 -> Views
 -> DI
 -> Migration
 -> SQL Server/SSMS
 -> Test
```
