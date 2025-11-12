using LibraryManagement.Api.Rest.Client;
using LibraryManagement.Api.Rest.Client.Domain.Authors;
using LibraryManagement.Api.Rest.Client.Domain.Authors.Create;

using Microsoft.AspNetCore.Components;

namespace LibraryManagement.Web.Components.Pages;

public partial class Author(IRestAPiClient restApiClient) : ComponentBase
{
    protected string AuthorName { get; set; } = string.Empty;
    protected AuthorDto? LastCreatedAuthor { get; set; }
    protected bool IsSubmitting { get; set; }
    protected string? ErrorMessage { get; set; }

    protected bool IsCreateDisabled => IsSubmitting || string.IsNullOrWhiteSpace(AuthorName);

    protected async Task CreateAuthor()
    {
        if (IsCreateDisabled)
        {
            return;
        }

        ErrorMessage = null;
        IsSubmitting = true;

        try
        {
            LastCreatedAuthor = await restApiClient.Authors.Create(new CreateAuthorRequestDto(AuthorName));
            AuthorName = string.Empty;
        }
        catch (Exception)
        {
            ErrorMessage = "Failed to create author. Please try again.";
        }
        finally
        {
            IsSubmitting = false;
        }
    }
}
