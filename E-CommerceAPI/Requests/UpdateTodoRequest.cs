namespace CleanArchitecture_CQRS_inAction.Requests
{
    public class UpdateTodoRequest
    {
        public string Title { get; set; } = string.Empty;
        public bool Cpmpleted { get; set; }
    }
}
