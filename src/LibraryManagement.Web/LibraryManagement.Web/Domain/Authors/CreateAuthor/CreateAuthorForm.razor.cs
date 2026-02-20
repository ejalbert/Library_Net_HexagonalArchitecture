using Microsoft.AspNetCore.Components;

namespace LibraryManagement.Web.Domain.Authors.CreateAuthor;

public partial class CreateAuthorForm : ComponentBase
{
    private const string ComponentName = "author-form";

    [Parameter][EditorRequired] public AuthorModel Model { get; set; } = new();

    [Parameter][EditorRequired] public string SubmitButtonText { get; set; } = string.Empty;

    [Parameter][EditorRequired] public EventCallback<AuthorModel> OnSubmit { get; set; }

    [Parameter] public string? FormName { get; set; }


    private async Task HandleSubmit()
    {
        if (OnSubmit.HasDelegate) await OnSubmit.InvokeAsync(Model);
    }
}
