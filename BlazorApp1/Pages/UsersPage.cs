using System.Net.Http.Json;
using System.Text.Json;
using ClassLibrary1;
using Microsoft.AspNetCore.Components;

namespace BlazorApp1.Pages;

public class UsersPages : ComponentBase
{
    public UsersModel? Users { get; set; }

    [Inject]
    public HttpClient HttpClient { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Users = await HttpClient.GetFromJsonAsync<UsersModel>("/api/User");
    }
}