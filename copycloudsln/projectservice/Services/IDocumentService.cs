namespace projectservice.Services
{
    public interface IDocumentService
    {
        Task<Tuple<bool, string>> CreateDocument(string projectId);
        Task<Tuple<bool, string>> SaveDocument(string content, string projectId);
        Task<string> GetDocumentContent(string projectId);
    }
}
