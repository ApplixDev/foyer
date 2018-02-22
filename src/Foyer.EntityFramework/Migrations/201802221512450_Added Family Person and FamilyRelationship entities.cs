namespace Foyer.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFamilyPersonandFamilyRelationshipentities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Families",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeadOfFamilyId = c.Int(nullable: false),
                        FamilyName = c.String(maxLength: 32),
                        OtherDetails = c.String(),
                        WidingDate = c.DateTime(),
                        DivorceDate = c.DateTime(),
                        EntryDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Family_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.HeadOfFamilyId, cascadeDelete: true)
                .Index(t => t.HeadOfFamilyId);
            
            CreateTable(
                "dbo.FamilyRelationships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FamilyId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        RelatedPersonId = c.Int(nullable: false),
                        RelationshipType = c.Byte(nullable: false),
                        PersonRole = c.Byte(nullable: false),
                        RelatedPersonRole = c.Byte(nullable: false),
                        OtherDetails = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FamilyRelationship_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Families", t => t.FamilyId, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.PersonId)
                .ForeignKey("dbo.People", t => t.RelatedPersonId)
                .Index(t => t.FamilyId)
                .Index(t => t.PersonId)
                .Index(t => t.RelatedPersonId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 32),
                        LastName = c.String(nullable: false, maxLength: 32),
                        Gender = c.Byte(nullable: false),
                        DateOfBirth = c.DateTime(),
                        PlaceOfBirth = c.DateTime(),
                        OtherDetails = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Person_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Families", "HeadOfFamilyId", "dbo.People");
            DropForeignKey("dbo.FamilyRelationships", "RelatedPersonId", "dbo.People");
            DropForeignKey("dbo.FamilyRelationships", "PersonId", "dbo.People");
            DropForeignKey("dbo.FamilyRelationships", "FamilyId", "dbo.Families");
            DropIndex("dbo.FamilyRelationships", new[] { "RelatedPersonId" });
            DropIndex("dbo.FamilyRelationships", new[] { "PersonId" });
            DropIndex("dbo.FamilyRelationships", new[] { "FamilyId" });
            DropIndex("dbo.Families", new[] { "HeadOfFamilyId" });
            DropTable("dbo.People",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Person_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FamilyRelationships",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FamilyRelationship_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Families",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Family_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
