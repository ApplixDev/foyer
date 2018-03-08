namespace Foyer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Husband_And_Wife_To_Family_Entity : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Families", name: "HeadOfFamilyId", newName: "HusbandId");
            RenameIndex(table: "dbo.Families", name: "IX_HeadOfFamilyId", newName: "IX_HusbandId");
            AddColumn("dbo.Families", "WifeId", c => c.Int());
            CreateIndex("dbo.Families", "WifeId");
            AddForeignKey("dbo.Families", "WifeId", "dbo.People", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Families", "WifeId", "dbo.People");
            DropIndex("dbo.Families", new[] { "WifeId" });
            DropColumn("dbo.Families", "WifeId");
            RenameIndex(table: "dbo.Families", name: "IX_HusbandId", newName: "IX_HeadOfFamilyId");
            RenameColumn(table: "dbo.Families", name: "HusbandId", newName: "HeadOfFamilyId");
        }
    }
}
