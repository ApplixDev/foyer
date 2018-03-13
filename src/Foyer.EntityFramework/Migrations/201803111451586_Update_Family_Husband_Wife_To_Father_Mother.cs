namespace Foyer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Family_Husband_Wife_To_Father_Mother : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Families", name: "HusbandId", newName: "FatherId");
            RenameColumn(table: "dbo.Families", name: "WifeId", newName: "MotherId");
            RenameIndex(table: "dbo.Families", name: "IX_HusbandId", newName: "IX_FatherId");
            RenameIndex(table: "dbo.Families", name: "IX_WifeId", newName: "IX_MotherId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Families", name: "IX_MotherId", newName: "IX_WifeId");
            RenameIndex(table: "dbo.Families", name: "IX_FatherId", newName: "IX_HusbandId");
            RenameColumn(table: "dbo.Families", name: "MotherId", newName: "WifeId");
            RenameColumn(table: "dbo.Families", name: "FatherId", newName: "HusbandId");
        }
    }
}
