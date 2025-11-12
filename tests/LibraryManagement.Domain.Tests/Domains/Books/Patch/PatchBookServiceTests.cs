using System;
using System.Linq;
using LibraryManagement.Domain.Domains.Books;
using LibraryManagement.Domain.Domains.Books.Patch;
using Moq;

namespace LibraryManagement.Domain.Tests.Domains.Books.Patch;

public class PatchBookServiceTests
{
    [Fact]
    public async Task Patch_passes_fields_to_port_and_returns_patched_book()
    {
        Mock<IPatchBookPort> portMock = new();
        Book patched = new()
        {
            Id = "book-1",
            Title = "New Title",
            AuthorId = "author-1",
            Description = "Updated",
            Keywords = new[] { "kw" }
        };

        portMock.Setup(port => port.Patch("book-1", null, null, "Updated", It.Is<IReadOnlyCollection<string>>(k => k.SequenceEqual(new[] { "kw" }))))
            .ReturnsAsync(patched);

        PatchBookService service = new(portMock.Object);

        Book result = await service.Patch(new PatchBookCommand("book-1", Description: "Updated", Keywords: new[] { "kw" }));

        Assert.Same(patched, result);
        portMock.Verify(port => port.Patch("book-1", null, null, "Updated", It.Is<IReadOnlyCollection<string>>(k => k.SequenceEqual(new[] { "kw" }))), Times.Once);
    }

    [Fact]
    public async Task Patch_without_any_fields_throws()
    {
        PatchBookService service = new(Mock.Of<IPatchBookPort>());

        await Assert.ThrowsAsync<ArgumentException>(() => service.Patch(new PatchBookCommand("book-1")));
    }
}
