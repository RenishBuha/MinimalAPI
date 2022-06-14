using Microsoft.EntityFrameworkCore;
using MinimalAPI.Models;

namespace MinimalAPI.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> option) : base(option)
        {
        }

        public DbSet<TodoItems> TodoItems => Set<TodoItems>();
    }
}