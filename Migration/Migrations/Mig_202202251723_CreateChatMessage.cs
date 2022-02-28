using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Migrations
{
    [Migration(202202251723)]
    public class Mig_202202251723_CreateChatMessage : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("ChatMessages")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("ToId").AsInt64().NotNullable()
                .WithColumn("FromId").AsInt64().NotNullable()
                .WithColumn("Message").AsString().NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable();

            Create.ForeignKey("ToIdFK") // You can give the FK a name or just let Fluent Migrator default to one
                .FromTable("ChatMessages").ForeignColumn("ToId")
                .ToTable("Users").PrimaryColumn("Id");

            Create.ForeignKey("FromIdFK") // You can give the FK a name or just let Fluent Migrator default to one
                .FromTable("ChatMessages").ForeignColumn("FromId")
                .ToTable("Users").PrimaryColumn("Id");
        }
        public override void Down()
        {
            Delete.ForeignKey("FromIdFK");
            Delete.ForeignKey("ToIdFK");
            Delete.Table("ChatMessage");
        }
    }
}
