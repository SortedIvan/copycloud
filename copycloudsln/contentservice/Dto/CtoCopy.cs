namespace contentservice.Dto
{
    public class CtoCopy
    {
        public string Id { get; set; } = string.Empty;
        public string Copy { get; set; } = string.Empty;
        public string CopyTone { get; set; } = string.Empty;
        public string CopyAction { get; set; } = string.Empty; 
        public string CopyContext { get; set; } = string.Empty;
    }

    public class CopyData
    {
        public List<Copy> Copies { get; set; } = new List<Copy>();
    }

    public class Copy
    {
        public string CopyText { get; set; } = string.Empty;
    }


}
