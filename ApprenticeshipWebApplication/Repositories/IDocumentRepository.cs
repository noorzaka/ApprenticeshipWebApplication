using ApprenticeshipWebApplication.Entities;

namespace ApprenticeshipWebApplication.Repositories
{
    public interface IDocumentRepository
    {
        public Task AddDocument(Document document);
        public List<Document> GetAssignmentDocuments(int assignmentId);
        public Document GetDoc(int documentId);
        public Task DeleteDoc(int documentId);
        public List<Document> GetReportDocuments(int reportId);
        Task AddDocument(Document document, int reportLogId);





    }
}
