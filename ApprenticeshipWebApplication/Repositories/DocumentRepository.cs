using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApprenticeshipWebApplication.Data;
using ApprenticeshipWebApplication.Entities; 
using Microsoft.EntityFrameworkCore;

namespace ApprenticeshipWebApplication.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext context;

        public DocumentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddDocument(Document document)
        {
            context.documents.Add(document);  
            await context.SaveChangesAsync();
        }
        public async Task AddDocument(Document document, int reportLogId)
        {
            document.reportLogId = reportLogId;  
            context.documents.Add(document);
            await context.SaveChangesAsync();
        }


        public List<Document> GetAssignmentDocuments(int assignmentId)
        {
            return context.documents.Where(x => x.assignmentId == assignmentId).ToList();  // Adjusted to the correct property name
        }

        public Document GetDoc(int documentId)
        {
            return context.documents.SingleOrDefault(x => x.documentId == documentId);  // Adjusted to the correct property name
        }
        public async Task DeleteDoc(int documentId)
        {
            var document = await context.documents.FindAsync(documentId);


            context.documents.Remove(document);
            await context.SaveChangesAsync();

        }
        public List<Document> GetReportDocuments(int reportId)
        {
            return context.documents
                .Include(d => d.reportLog)
                .Where(d => d.reportLog.reportId == reportId)
                .ToList();
        }
      

    }
}