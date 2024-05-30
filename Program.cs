using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using PizzaStoreNew.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Pizzas")?? "Data Source=pizzas.db";
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<PizzaDb>(connectionString);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PizzaStoreNew API", Description="Making the Pizzas you like", Version = "v1" });
});

var app = builder.Build();
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStoreNew API v1"));
}

app.MapGet("/", () => "Welcome to willa's Pizza Store!");
app.MapGet("/pizzas", async (PizzaDb db) => await db.Pizzas.ToListAsync());
app.MapGet("/pizzas/{id}", async (int id, PizzaDb db) => await db.Pizzas.FindAsync(id));
app.MapPost("/pizzas", async (Pizza pizza, PizzaDb db) => 
{
    await db.Pizzas.AddAsync(pizza);
    await db.SaveChangesAsync();
    return Results.Created($"/pizzas/{pizza.Id}", pizza);
});
app.MapPut("/pizzas/{id}", async (PizzaDb db, Pizza updatePizza, int id) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if(pizza is null){
        return Results.NotFound();
    }
    pizza.Name = updatePizza.Name;
    pizza.Description = updatePizza.Description;
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapDelete("/pizzas/{id}", async (PizzaDb db, int id) =>
{
    var pizza = await db.Pizzas.FindAsync(id);
    if(pizza is null){
        return Results.NotFound();
    }
    db.Pizzas.Remove(pizza);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.Run();
