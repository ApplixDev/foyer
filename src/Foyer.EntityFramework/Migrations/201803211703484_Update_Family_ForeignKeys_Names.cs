namespace Foyer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Family_ForeignKeys_Names : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Families", "HeadOfFamilyId", "dbo.People");
            DropForeignKey("dbo.Families", "WifeId", "dbo.People");
            AddForeignKey("dbo.Families", "FatherId", "dbo.People", "Id");
            AddForeignKey("dbo.Families", "MotherId", "dbo.People", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Families", "HeadOfFamilyId", "dbo.People");
            DropForeignKey("dbo.Families", "WifeId", "dbo.People");
            AddForeignKey("dbo.Families", "HeadOfFamilyId", "dbo.People", "Id");
            AddForeignKey("dbo.Families", "WifeId", "dbo.People", "Id");
        }
    }
}
