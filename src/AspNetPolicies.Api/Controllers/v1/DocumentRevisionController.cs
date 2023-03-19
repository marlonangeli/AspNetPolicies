using AspNetPolicies.Domain.Dtos;
using AspNetPolicies.Domain.Entities;
using AspNetPolicies.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetPolicies.Api.Controllers.v1;

public class DocumentRevisionController : ApiControllerBase
{
    private readonly IRepository<DocumentRevision> _repository;
    
    public DocumentRevisionController(IRepository<DocumentRevision> repository)
    {
        _repository = repository;
    }
    
    [HttpGet("")]
    public async Task<IActionResult> GetDocumentRevisions()
    {
        var documentRevisions = await _repository.GetAllAsync();
        return Ok(documentRevisions);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocumentRevision(int id)
    {
        var documentRevision = await _repository.GetQueryable()
            .Include(i => i.Document)
            .FirstOrDefaultAsync(x => x.Id == id);
        return Ok(documentRevision);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> CreateDocumentRevision(DocumentRevisionDto documentRevision)
    {
        var entity = new DocumentRevision
        {
            DocumentId = documentRevision.DocumentId,
            Content = documentRevision.Content,
            RevisionDate = documentRevision.RevisionDate,
            RevisionNumber = documentRevision.RevisionNumber
        };
        var createdDocumentRevision = await _repository.AddAsync(entity);
        return Ok(createdDocumentRevision);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDocumentRevision(int id, DocumentRevisionDto documentRevision)
    {
        var entity = new DocumentRevision
        {
            Id = id,
            DocumentId = documentRevision.DocumentId,
            Content = documentRevision.Content,
            RevisionDate = documentRevision.RevisionDate,
            RevisionNumber = documentRevision.RevisionNumber
        };
        var updatedDocumentRevision = await _repository.UpdateAsync(entity);
        return Ok(updatedDocumentRevision);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocumentRevision(int id)
    {
        var deletedDocumentRevision = await _repository.DeleteAsync(id);
        return Ok(deletedDocumentRevision);
    }
}