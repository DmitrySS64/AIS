namespace Сourse_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Catalogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CatalogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Catalogs", t => t.CatalogId, cascadeDelete: true)
                .Index(t => t.CatalogId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        specs = c.String(),
                        price = c.Int(nullable: false),
                        rating = c.Int(nullable: false),
                        reviewsCount = c.Int(nullable: false),
                        url = c.String(),
                        SectionId = c.Int(nullable: false),
                        SubsectionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sections", t => t.SectionId, cascadeDelete: true)
                .ForeignKey("dbo.Subsections", t => t.SubsectionId)
                .Index(t => t.SectionId)
                .Index(t => t.SubsectionId);
            
            CreateTable(
                "dbo.Subsections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SectionId = c.Int(nullable: false),
                        ParentSubsectionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subsections", t => t.ParentSubsectionId)
                .ForeignKey("dbo.Sections", t => t.SectionId, cascadeDelete: true)
                .Index(t => t.SectionId)
                .Index(t => t.ParentSubsectionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subsections", "SectionId", "dbo.Sections");
            DropForeignKey("dbo.Products", "SubsectionId", "dbo.Subsections");
            DropForeignKey("dbo.Subsections", "ParentSubsectionId", "dbo.Subsections");
            DropForeignKey("dbo.Products", "SectionId", "dbo.Sections");
            DropForeignKey("dbo.Sections", "CatalogId", "dbo.Catalogs");
            DropIndex("dbo.Subsections", new[] { "ParentSubsectionId" });
            DropIndex("dbo.Subsections", new[] { "SectionId" });
            DropIndex("dbo.Products", new[] { "SubsectionId" });
            DropIndex("dbo.Products", new[] { "SectionId" });
            DropIndex("dbo.Sections", new[] { "CatalogId" });
            DropTable("dbo.Subsections");
            DropTable("dbo.Products");
            DropTable("dbo.Sections");
            DropTable("dbo.Catalogs");
        }
    }
}
