namespace Foyer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFamilyEntryDateEntityProperty : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Families", "EntryDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Families", "EntryDate", c => c.DateTime());
        }
    }
}
