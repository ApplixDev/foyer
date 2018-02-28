namespace Foyer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePersonBirthPlaceEntityProperty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.People", "BirthPlace", c => c.String(maxLength: 328));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "BirthPlace", c => c.DateTime());
        }
    }
}
