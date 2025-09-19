using System.Net;
using System.Text;
using System.Text.Json;
using usecase_01_call_external_api.Abstractions;
using usecase_01_call_external_api.DTOs;
using usecase_01_call_external_api.Models;

namespace usecase_01_call_external_api.Clients;

public class ExternalApiClient
    (HttpClient client, ILogger<ExternalApiClient> logger) : IExternalApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<IEnumerable<Post>> GetPostsAsync(CancellationToken cancellationToken = default)
    {
        using var response = await client.GetAsync("posts", cancellationToken);
        
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var posts = JsonSerializer.Deserialize<List<Post>>(content, JsonOptions);
        return posts ?? new List<Post>();
    }
    public async Task<Post?> CreatePostAsync(CreatePostDto post, CancellationToken cancellationToken = default)
    {
        var payload = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("posts", payload, cancellationToken);
        
        if (!response.IsSuccessStatusCode) return null;
        
        response.EnsureSuccessStatusCode();
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return await JsonSerializer.DeserializeAsync<Post>(stream, JsonOptions, cancellationToken);
    }

    public async Task<Post?> GetPostByIdAsync(int postId, CancellationToken cancellationToken = default)
    {
        var response = await client.GetAsync($"posts/{postId}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            logger.LogWarning($"post with id {postId} not found.");
            return null;
        }
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        var post = JsonSerializer.Deserialize<Post>(json, JsonOptions);
        return post;
    }

    public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var response = await client.GetAsync("users",  cancellationToken);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var users = JsonSerializer.Deserialize<List<User>>(content, JsonOptions);
        return users ?? new List<User>();
    }
}