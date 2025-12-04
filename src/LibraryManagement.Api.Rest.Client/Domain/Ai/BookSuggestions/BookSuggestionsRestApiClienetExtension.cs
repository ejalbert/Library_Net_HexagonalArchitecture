namespace LibraryManagement.Api.Rest.Client.Domain.Ai.BookSuggestions;

public static class BookSuggestionsRestApiClienetExtension
{
    extension(IRestAPiClient source)
    {
        public IBookSuggestionClient BookSuggestions => new BookSuggestionClient(source);
    }
}
