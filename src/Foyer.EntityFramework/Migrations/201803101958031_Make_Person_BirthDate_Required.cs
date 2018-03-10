namespace Foyer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Make_Person_BirthDate_Required : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.People", "BirthDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "BirthDate", c => c.DateTime());
        }
    }
}
