using usecase_01_call_external_api.DTOs;
using usecase_01_call_external_api.Models;
namespace usecase_01_call_external_api.Abstractions;
public interface IExternalApiClient
{
    public Task<IEnumerable<Post>> GetPostsAsync(CancellationToken cancellationToken = default);
    public Task<Post?> CreatePostAsync(CreatePostDto post, CancellationToken cancellationToken = default);
    public Task<Post?> GetPostByIdAsync(int postId, CancellationToken cancellationToken = default);
    public Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken = default);
}