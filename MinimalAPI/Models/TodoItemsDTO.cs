namespace MinimalAPI.Models
{
    public class TodoItemsDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }

        public TodoItemsDTO(){}

        public TodoItemsDTO(TodoItems todo) =>
            (Id, Name, IsComplete) = (todo.Id, todo.Name, todo.IsComplete);
    }
}
