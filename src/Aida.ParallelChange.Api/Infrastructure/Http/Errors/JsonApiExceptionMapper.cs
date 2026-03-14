namespace Aida.ParallelChange.Api.Infrastructure.Http.Errors;

public interface JsonApiExceptionMapper
{
    bool CanHandle(Exception exception);

    JsonApiErrorResponse Map(Exception exception);
}
