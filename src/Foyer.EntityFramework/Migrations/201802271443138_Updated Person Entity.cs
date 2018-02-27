namespace Foyer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedPersonEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "BirthDate", c => c.DateTime());
            AddColumn("dbo.People", "BirthPlace", c => c.DateTime());
            DropColumn("dbo.People", "DateOfBirth");
            DropColumn("dbo.People", "PlaceOfBirth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.People", "PlaceOfBirth", c => c.DateTime());
            AddColumn("dbo.People", "DateOfBirth", c => c.DateTime());
            DropColumn("dbo.People", "BirthPlace");
            DropColumn("dbo.People", "BirthDate");
        }
    }
}
