using System.Text.Json;
using ClassLibrary1;
using Microsoft.AspNetCore.Components;

namespace BlazorApp1.Pages;

public class UsersPages : ComponentBase
{
    public List<UserDto> Users { get; set; } // или public UsersModel Users { get; set; }

    [Inject]
    public HttpClient HttpClient { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var response = await HttpClient.GetAsync("/api/user");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<UserDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Users = users;
        }
    }
}
