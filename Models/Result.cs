namespace Models
{
    public class Result<T>
    {
        public bool Correct { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public T? Object { get; set; } 
        public List<T>? Objects { get; set; } 
        public Exception? Ex { get; set; }
    }
}
