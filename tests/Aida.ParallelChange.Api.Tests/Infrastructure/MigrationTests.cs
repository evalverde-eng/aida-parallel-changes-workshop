using Aida.ParallelChange.Api.Infrastructure.Persistence.Migrations;
using FluentMigrator;
using Shouldly;

namespace Aida.ParallelChange.Api.Tests.Infrastructure;

[TestFixture]
public sealed class MigrationTests
{
    [Test]
    public void Migrations_include_initial_schema_version()
    {
        var migrationVersions = typeof(CreateCustomerContactsTable)
            .Assembly
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(Migration)))
            .Select(type => type.GetCustomAttributes(typeof(MigrationAttribute), inherit: false)
                .OfType<MigrationAttribute>()
                .Single()
                .Version)
            .OrderBy(version => version)
            .ToArray();

        migrationVersions.ShouldContain(202604010001);
    }

    [Test]
    public void Migration_versions_are_unique()
    {
        var migrationVersions = typeof(CreateCustomerContactsTable)
            .Assembly
            .GetTypes()
            .Where(type => type.IsSubclassOf(typeof(Migration)))
            .Select(type => type.GetCustomAttributes(typeof(MigrationAttribute), inherit: false)
                .OfType<MigrationAttribute>()
                .Single()
                .Version)
            .ToArray();

        migrationVersions.Distinct().Count().ShouldBe(migrationVersions.Length);
    }
}
