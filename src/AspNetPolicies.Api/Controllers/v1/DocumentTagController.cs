using AspNetPolicies.Domain.Dtos;
using AspNetPolicies.Domain.Entities;
using AspNetPolicies.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetPolicies.Api.Controllers.v1;

public class DocumentTagController : ApiControllerBase
{
    private readonly IRepository<DocumentTag> _repository;
    
    public DocumentTagController(IRepository<DocumentTag> repository)
    {
        _repository = repository;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetDocumentTags()
    {
        var documentTags = await _repository.GetAllAsync();
        return Ok(documentTags);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocumentTag(int id)
    {
        var documentTag = await _repository.GetQueryable()
            .Include(i => i.Document)
            .FirstOrDefaultAsync(x => x.DocumentId == id);
        return Ok(documentTag);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> CreateDocumentTag(DocumentTagDto documentTag)
    {
        var entity = new DocumentTag
        {
            DocumentId = documentTag.DocumentId,
            Tag = documentTag.Tag
        };
        var createdDocumentTag = await _repository.AddAsync(entity);
        return Ok(createdDocumentTag);
    }
    
    [HttpPut("")]
    public async Task<IActionResult> UpdateDocumentTag(int id, DocumentTagDto documentTag)
    {
        var entity = new DocumentTag
        {
            DocumentId = documentTag.DocumentId,
            Tag = documentTag.Tag
        };
        var updatedDocumentTag = await _repository.UpdateAsync(entity);
        return Ok(updatedDocumentTag);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocumentTag(int id)
    {
        var deletedDocumentTag = await _repository.DeleteAsync(id);
        return Ok(deletedDocumentTag);
    }
}