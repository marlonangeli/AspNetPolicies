using AspNetPolicies.Api.Constants;
using AspNetPolicies.Domain.Dtos;
using AspNetPolicies.Domain.Entities;
using AspNetPolicies.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetPolicies.Api.Controllers.v1;

public class DocumentController : ApiControllerBase
{
    private readonly IRepository<Document> _repository;
    private readonly IRepository<User> _userRepository;

    public DocumentController(IRepository<Document> repository, IRepository<User> userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }
    
    [HttpGet("")]
    public async Task<IActionResult> GetDocuments()
    {
        var documents = await _repository.GetAllAsync();
        return Ok(documents);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocument(int id)
    {
        var document = await _repository.GetQueryable()
            .Include(i => i.AuthorizedUsers)
            .Include(i => i.OwnerUser)
            .Include(i => i.DocumentCategories)
            .Include(i => i.DocumentRevisions)
            .Include(i => i.DocumentTags)
            .FirstOrDefaultAsync(x => x.Id == id);
        return Ok(document);
    }
    
    [Authorize(Policy = Policies.USER_OWNER_DOCUMENT)]
    [HttpGet("owner/{id}")]
    public async Task<IActionResult> GetDocumentOwner(int id)
    {
        var document = await _repository.GetQueryable()
            .Include(i => i.AuthorizedUsers)
            .Include(i => i.OwnerUser)
            .Include(i => i.DocumentCategories)
            .Include(i => i.DocumentRevisions)
            .Include(i => i.DocumentTags)
            .FirstOrDefaultAsync(x => x.Id == id);
        return Ok(document);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> CreateDocument(DocumentDto document)
    {
        var entity = new Document
        {
            Name = document.Name,
            Content = document.Content,
            OwnerUserId = document.OwnerUserId
        };
        var createdDocument = await _repository.AddAsync(entity);
        return Ok(createdDocument);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDocument(int id, DocumentDto document)
    {
        var entity = new Document
        {
            Id = id,
            Name = document.Name,
            Content = document.Content,
            OwnerUserId = document.OwnerUserId
        };
        var updatedDocument = await _repository.UpdateAsync(entity);
        return Ok(updatedDocument);
    }
    
    [HttpPut("{id}/add-user/{userId}")]
    public async Task<IActionResult> AddUserToDocument(int id, int userId)
    {
        var document = await _repository.GetQueryable()
            .Include(i => i.AuthorizedUsers)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (document == null)
            return NotFound();
        if (document.AuthorizedUsers.Any(x => x.Id == userId) || document.OwnerUserId == userId)
            return BadRequest("User already exists in document");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound();
        
        document.AuthorizedUsers.Add(user);
        
        var updatedDocument = await _repository.UpdateAsync(document);
        return Ok(updatedDocument);
    }
    
    [HttpPut("{id}/remove-user/{userId}")]
    public async Task<IActionResult> RemoveUserFromDocument(int id, int userId)
    {
        var document = await _repository.GetQueryable()
            .Include(i => i.AuthorizedUsers)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (document == null)
            return NotFound();
        if (document.AuthorizedUsers.All(x => x.Id != userId) || document.OwnerUserId == userId)
            return BadRequest("User does not exist in document");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound();
        
        document.AuthorizedUsers.Remove(user);
        
        var updatedDocument = await _repository.UpdateAsync(document);
        return Ok(updatedDocument);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDocument(int id)
    {
        var deletedDocument = await _repository.DeleteAsync(id);
        return Ok(deletedDocument);
    }
}