namespace Сourse_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sections", "CatalogId", "dbo.Catalogs");
            DropForeignKey("dbo.Products", "SectionId", "dbo.Sections");
            DropForeignKey("dbo.Subsections", "SectionId", "dbo.Sections");
            DropIndex("dbo.Sections", new[] { "CatalogId" });
            DropIndex("dbo.Products", new[] { "SectionId" });
            DropIndex("dbo.Subsections", new[] { "SectionId" });
            AddColumn("dbo.Subsections", "CatalogId", c => c.Int(nullable: false));
            CreateIndex("dbo.Subsections", "CatalogId");
            AddForeignKey("dbo.Subsections", "CatalogId", "dbo.Catalogs", "Id", cascadeDelete: true);
            DropColumn("dbo.Subsections", "SectionId");
            DropTable("dbo.Sections");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CatalogId = c.Int(nullable: false),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Subsections", "SectionId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Subsections", "CatalogId", "dbo.Catalogs");
            DropIndex("dbo.Subsections", new[] { "CatalogId" });
            DropColumn("dbo.Subsections", "CatalogId");
            CreateIndex("dbo.Subsections", "SectionId");
            CreateIndex("dbo.Products", "SectionId");
            CreateIndex("dbo.Sections", "CatalogId");
            AddForeignKey("dbo.Subsections", "SectionId", "dbo.Sections", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Products", "SectionId", "dbo.Sections", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Sections", "CatalogId", "dbo.Catalogs", "Id", cascadeDelete: true);
        }
    }
}
