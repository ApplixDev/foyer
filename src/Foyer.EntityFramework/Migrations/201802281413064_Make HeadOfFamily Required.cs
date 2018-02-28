namespace Foyer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeHeadOfFamilyRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Families", "HeadOfFamilyId", "dbo.People");
            DropIndex("dbo.Families", new[] { "HeadOfFamilyId" });
            AlterColumn("dbo.Families", "HeadOfFamilyId", c => c.Int(nullable: false));
            CreateIndex("dbo.Families", "HeadOfFamilyId");
            AddForeignKey("dbo.Families", "HeadOfFamilyId", "dbo.People", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Families", "HeadOfFamilyId", "dbo.People");
            DropIndex("dbo.Families", new[] { "HeadOfFamilyId" });
            AlterColumn("dbo.Families", "HeadOfFamilyId", c => c.Int());
            CreateIndex("dbo.Families", "HeadOfFamilyId");
            AddForeignKey("dbo.Families", "HeadOfFamilyId", "dbo.People", "Id");
        }
    }
}
