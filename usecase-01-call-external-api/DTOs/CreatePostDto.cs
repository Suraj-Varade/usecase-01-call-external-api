namespace usecase_01_call_external_api.DTOs;

public class CreatePostDto
{
    public int UserId { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
}