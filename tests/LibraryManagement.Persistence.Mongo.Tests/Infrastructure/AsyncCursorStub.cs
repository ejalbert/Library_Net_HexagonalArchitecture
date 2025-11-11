using MongoDB.Driver;

namespace LibraryManagement.Persistence.Mongo.Tests.Infrastructure;

internal sealed class AsyncCursorStub<TDocument> : IAsyncCursor<TDocument>, IEnumerable<TDocument>
{
    private readonly IEnumerator<TDocument> _enumerator;
    private bool _isDisposed;

    public AsyncCursorStub(IEnumerable<TDocument> documents)
    {
        _enumerator = (documents ?? Enumerable.Empty<TDocument>()).GetEnumerator();
    }

    public IEnumerable<TDocument> Current { get; private set; } = Enumerable.Empty<TDocument>();

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        _enumerator.Dispose();
    }

    public bool MoveNext(CancellationToken cancellationToken = default)
    {
        EnsureNotDisposed();

        if (!_enumerator.MoveNext())
        {
            Current = Enumerable.Empty<TDocument>();
            return false;
        }

        Current = new[] { _enumerator.Current };
        return true;
    }

    public Task<bool> MoveNextAsync(CancellationToken cancellationToken) => Task.FromResult(MoveNext(cancellationToken));

    public IEnumerator<TDocument> GetEnumerator()
    {
        EnsureNotDisposed();

        while (MoveNext())
        {
            foreach (TDocument document in Current)
            {
                yield return document;
            }
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    private void EnsureNotDisposed()
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException(nameof(AsyncCursorStub<TDocument>));
        }
    }
}
