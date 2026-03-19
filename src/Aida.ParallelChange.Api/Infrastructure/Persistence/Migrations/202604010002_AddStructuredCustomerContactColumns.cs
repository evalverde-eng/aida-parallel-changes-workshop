using FluentMigrator;

namespace Aida.ParallelChange.Api.Infrastructure.Persistence.Migrations;

[Migration(202604010002)]
public sealed class AddStructuredCustomerContactColumns : Migration
{
    public override void Up()
    {
        Alter.Table("customer_contacts")
            .AddColumn("first_name").AsString(100).Nullable()
            .AddColumn("last_name").AsString(100).Nullable()
            .AddColumn("phone_country_code").AsString(10).Nullable()
            .AddColumn("phone_local_number").AsString(30).Nullable();
    }

    public override void Down()
    {
        Delete.Column("phone_local_number").FromTable("customer_contacts");
        Delete.Column("phone_country_code").FromTable("customer_contacts");
        Delete.Column("last_name").FromTable("customer_contacts");
        Delete.Column("first_name").FromTable("customer_contacts");
    }
}
