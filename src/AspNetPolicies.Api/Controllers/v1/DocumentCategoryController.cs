using AspNetPolicies.Api.Constants;
using AspNetPolicies.Domain.Dtos;
using AspNetPolicies.Domain.Entities;
using AspNetPolicies.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetPolicies.Api.Controllers.v1;

public class DocumentCategoryController : ApiControllerBase
{
    private readonly IRepository<DocumentCategory> _repository;
    
    public DocumentCategoryController(IRepository<DocumentCategory> repository)
    {
        _repository = repository;
    }
    
    [Authorize(Roles = Roles.READ)]
    [HttpGet("")]
    public async Task<IActionResult> GetDocumentCategories()
    {
        var documentCategories = await _repository.GetAllAsync();
        return Ok(documentCategories);
    }
    
    [Authorize(Roles = Roles.READ)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocumentCategory(int id)
    {
        var documentCategory = await _repository.GetQueryable()
            .Include(i => i.Document)
            .FirstOrDefaultAsync(x => x.DocumentId == id);
        return Ok(documentCategory);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> CreateDocumentCategory(DocumentCategoryDto documentCategory)
    {
        var entity = new DocumentCategory
        {
            DocumentId = documentCategory.DocumentId,
            Category = documentCategory.Category
        };
        var createdDocumentCategory = await _repository.AddAsync(entity);
        return Ok(createdDocumentCategory);
    }
    
    [HttpPut("")]
    public async Task<IActionResult> UpdateDocumentCategory(DocumentCategoryDto documentCategory)
    {
        var entity = new DocumentCategory
        {
            DocumentId = documentCategory.DocumentId,
            Category = documentCategory.Category
        };
        var updatedDocumentCategory = await _repository.UpdateAsync(entity);
        return Ok(updatedDocumentCategory);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocumentCategory(int id)
    {
        var deletedDocumentCategory = await _repository.DeleteAsync(id);
        return Ok(deletedDocumentCategory);
    }
}