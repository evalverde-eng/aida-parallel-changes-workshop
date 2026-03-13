namespace Aida.ParallelChange.Api.Contracts.JsonApi;

public class JsonApiDataDocument<TAttributes>
{
    public JsonApiResource<TAttributes> Data { get; init; } = default!;
}
