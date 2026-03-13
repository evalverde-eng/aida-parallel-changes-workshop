namespace Aida.ParallelChange.Api.Contracts.JsonApi;

public sealed class JsonApiDataDocument<TAttributes>
{
    public JsonApiResource<TAttributes> Data { get; init; } = default!;
}
