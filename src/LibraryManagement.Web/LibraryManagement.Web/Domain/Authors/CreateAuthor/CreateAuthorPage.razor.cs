using LibraryManagement.Api.Rest.Client.Generated.Wrapper;

using Microsoft.AspNetCore.Components;

namespace LibraryManagement.Web.Domain.Authors.CreateAuthor;

public partial class CreateAuthorPage(IRestApiClient restApiClient, IAuthorModelMapper mapper) : ComponentBase
{
    private AuthorModel? FormModel { get; set; }

    private AuthorModel? LastCreatedAuthor { get; set; }
    private bool IsSubmitting { get; set; }
    private string? ErrorMessage { get; set; }

    private bool IsCreateDisabled => IsSubmitting || string.IsNullOrWhiteSpace(FormModel?.Name ?? string.Empty);

    protected override void OnInitialized()
    {
        FormModel ??= new AuthorModel();
    }

    protected async Task CreateAuthor(AuthorModel newAuthor)
    {
        if (IsCreateDisabled) return;

        ErrorMessage = null;
        IsSubmitting = true;

        try
        {
            var createAuthorRequestDto = mapper.ToCreateAuthorRequestDto(FormModel!);
            LastCreatedAuthor = mapper.ToModel(await restApiClient.Authors.CreateAuthorAsync(createAuthorRequestDto));
            FormModel = new AuthorModel();
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
