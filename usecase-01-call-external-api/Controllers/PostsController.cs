using Microsoft.AspNetCore.Mvc;
using usecase_01_call_external_api.Abstractions;
using usecase_01_call_external_api.DTOs;
using usecase_01_call_external_api.Models;

namespace usecase_01_call_external_api.Controllers;

public class PostsController(IExternalApiClient apiClient) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Post>>> GetPosts(CancellationToken ct)
    {
        var posts = await apiClient.GetPostsAsync(ct);
        return Ok(posts);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Post>> GetPost(int id, CancellationToken ct)
    {
        var post = await apiClient.GetPostByIdAsync(id, ct);
        if (post == null)
        {
            return NotFound("Post not found");
        }
        return Ok(post);
    }
    
    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost([FromBody] CreatePostDto post, CancellationToken ct)
    {
        var created = await apiClient.CreatePostAsync(post, ct);
        if (created == null)
        {
            return BadRequest("Unable to create post.");
        }
        return CreatedAtAction(nameof(GetPost), new { id = created.Id }, created);
    }
}