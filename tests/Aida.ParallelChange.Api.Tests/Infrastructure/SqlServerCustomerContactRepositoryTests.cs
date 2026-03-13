using Aida.ParallelChange.Api.Infrastructure.Persistence.SqlServer;
using Shouldly;

namespace Aida.ParallelChange.Api.Tests.Infrastructure;

[TestFixture]
public sealed class SqlServerCustomerContactRepositoryTests
{
    [Test]
    public void FindById_query_reads_identity_and_email_fields()
    {
        SqlQueries.FindById.ShouldContain("customer_id");
        SqlQueries.FindById.ShouldContain("email");
        SqlQueries.FindById.ShouldContain("WHERE customer_id = @CustomerId");
    }

    [Test]
    public void Upsert_query_uses_merge_with_update_and_insert_paths()
    {
        SqlQueries.Upsert.ShouldContain("MERGE customer_contacts");
        SqlQueries.Upsert.ShouldContain("WHEN MATCHED");
        SqlQueries.Upsert.ShouldContain("WHEN NOT MATCHED");
        SqlQueries.Upsert.ShouldContain("@CustomerId");
        SqlQueries.Upsert.ShouldContain("@Email");
    }
}
