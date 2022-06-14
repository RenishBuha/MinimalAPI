using Microsoft.EntityFrameworkCore;
using MinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

//https://localhost:7235/
app.MapGet("/", () => "Hello World");

//https://localhost:7235/GetTodoItems
app.MapGet("/GetTodoItems", async (TodoContext db) => 
    await db.TodoItems.Select(x => new TodoItemsDTO(x)).ToListAsync());

//https://localhost:7235/GetTodoItems/1
app.MapGet("/GetTodoItems/{id}", async (int id, TodoContext db) => 
    await db.TodoItems.FindAsync(id) is TodoItems items ? Results.Ok(new TodoItemsDTO(items)) : Results.NotFound());

//https://localhost:7235/PostTodoItems
app.MapPost("/PostTodoItems", async (TodoItemsDTO items, TodoContext db) =>
{
    var todo = new TodoItems
    {
        Name = items.Name,
        IsComplete = items.IsComplete
    };

    db.TodoItems.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/DisplayTodoItems/{todo.Id}", new TodoItemsDTO(todo));
});

//https://localhost:7235/PutTodoItems/1
app.MapPut("/PutTodoItems/{id}", async (int id, TodoItemsDTO inputTodo, TodoContext db) =>
{
    var todo = await db.TodoItems.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.Created($"/DisplayTodoItems/{id}", todo);
    //return Results.NoContent();
});

//https://localhost:7235/DeleteTodoItems/1
app.MapDelete("/DeleteTodoItems/{id}", async (int id, TodoContext db) =>
{
    if (await db.TodoItems.FindAsync(id) is TodoItems todo)
    {
        db.TodoItems.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }

    return Results.NotFound();
});


app.Run();