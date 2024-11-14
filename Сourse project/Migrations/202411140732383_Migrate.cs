namespace Сourse_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Subsections", "CatalogId", "dbo.Catalogs");
            DropForeignKey("dbo.Subsections", "ParentSubsectionId", "dbo.Subsections");
            DropForeignKey("dbo.Products", "SubsectionId", "dbo.Subsections");
            DropIndex("dbo.Subsections", new[] { "CatalogId" });
            DropIndex("dbo.Subsections", new[] { "ParentSubsectionId" });
            DropIndex("dbo.Products", new[] { "SubsectionId" });
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        BrandId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brands", t => t.BrandId, cascadeDelete: true)
                .Index(t => t.BrandId);
            
            CreateTable(
                "dbo.Models",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LineId = c.Int(nullable: false),
                        MatrixTypeId = c.Int(nullable: false),
                        ScreenResolutionFormatId = c.Int(nullable: false),
                        ScreenRefreshRateId = c.Int(nullable: false),
                        OSId = c.Int(nullable: false),
                        ProcessorId = c.Int(nullable: false),
                        Lifespan = c.String(),
                        ScreenSize = c.String(),
                        ScreenResolutionId = c.Int(nullable: false),
                        SIMCount = c.Int(nullable: false),
                        BatteryCapacity = c.String(),
                        ChargingTime = c.String(),
                        HasHeadphoneJack = c.String(),
                        ReleaseDate = c.String(),
                        CasingMaterialId = c.Int(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Features = c.String(),
                        VersionType_Id = c.Int(),
                        TypeOfMatrix_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VersionTypes", t => t.VersionType_Id)
                .ForeignKey("dbo.CasingMaterials", t => t.CasingMaterialId, cascadeDelete: true)
                .ForeignKey("dbo.Lines", t => t.LineId, cascadeDelete: true)
                .ForeignKey("dbo.OS", t => t.OSId, cascadeDelete: true)
                .ForeignKey("dbo.Processors", t => t.ProcessorId, cascadeDelete: true)
                .ForeignKey("dbo.ScreenRefreshRates", t => t.ScreenRefreshRateId, cascadeDelete: true)
                .ForeignKey("dbo.ScreenResolutions", t => t.ScreenResolutionId, cascadeDelete: true)
                .ForeignKey("dbo.ScreenResolutionFormats", t => t.ScreenResolutionFormatId, cascadeDelete: true)
                .ForeignKey("dbo.TypeOfMatrices", t => t.TypeOfMatrix_Id)
                .Index(t => t.LineId)
                .Index(t => t.ScreenResolutionFormatId)
                .Index(t => t.ScreenRefreshRateId)
                .Index(t => t.OSId)
                .Index(t => t.ProcessorId)
                .Index(t => t.ScreenResolutionId)
                .Index(t => t.CasingMaterialId)
                .Index(t => t.VersionType_Id)
                .Index(t => t.TypeOfMatrix_Id);
            
            CreateTable(
                "dbo.CasingMaterials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Smartphones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(),
                        ArticleNumber = c.String(),
                        Title = c.String(),
                        Warranty = c.String(),
                        Price = c.Int(nullable: false),
                        ModelId = c.Int(nullable: false),
                        ColorTypeId = c.Int(nullable: false),
                        VersionTypeId = c.Int(nullable: false),
                        RAMSizeId = c.Int(nullable: false),
                        BuiltInMemoryId = c.Int(nullable: false),
                        CasingMaterial_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BuiltInMemories", t => t.BuiltInMemoryId, cascadeDelete: true)
                .ForeignKey("dbo.ColorTypes", t => t.ColorTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Models", t => t.ModelId, cascadeDelete: true)
                .ForeignKey("dbo.RAMs", t => t.RAMSizeId, cascadeDelete: true)
                .ForeignKey("dbo.VersionTypes", t => t.VersionTypeId, cascadeDelete: true)
                .ForeignKey("dbo.CasingMaterials", t => t.CasingMaterial_Id)
                .Index(t => t.ModelId)
                .Index(t => t.ColorTypeId)
                .Index(t => t.VersionTypeId)
                .Index(t => t.RAMSizeId)
                .Index(t => t.BuiltInMemoryId)
                .Index(t => t.CasingMaterial_Id);
            
            CreateTable(
                "dbo.BuiltInMemories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ColorTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RAMs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Size = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VersionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OS",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Processors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScreenRefreshRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScreenResolutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Resolution = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScreenResolutionFormats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TypeOfMatrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.Catalogs");
            DropTable("dbo.Subsections");
            DropTable("dbo.Products");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Specs = c.String(),
                        Price = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        ReviewsCount = c.Int(nullable: false),
                        Url = c.String(),
                        SectionId = c.Int(nullable: false),
                        SubsectionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subsections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Url = c.String(),
                        CatalogId = c.Int(nullable: false),
                        ParentSubsectionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Catalogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Models", "TypeOfMatrix_Id", "dbo.TypeOfMatrices");
            DropForeignKey("dbo.Models", "ScreenResolutionFormatId", "dbo.ScreenResolutionFormats");
            DropForeignKey("dbo.Models", "ScreenResolutionId", "dbo.ScreenResolutions");
            DropForeignKey("dbo.Models", "ScreenRefreshRateId", "dbo.ScreenRefreshRates");
            DropForeignKey("dbo.Models", "ProcessorId", "dbo.Processors");
            DropForeignKey("dbo.Models", "OSId", "dbo.OS");
            DropForeignKey("dbo.Models", "LineId", "dbo.Lines");
            DropForeignKey("dbo.Models", "CasingMaterialId", "dbo.CasingMaterials");
            DropForeignKey("dbo.Smartphones", "CasingMaterial_Id", "dbo.CasingMaterials");
            DropForeignKey("dbo.Smartphones", "VersionTypeId", "dbo.VersionTypes");
            DropForeignKey("dbo.Models", "VersionType_Id", "dbo.VersionTypes");
            DropForeignKey("dbo.Smartphones", "RAMSizeId", "dbo.RAMs");
            DropForeignKey("dbo.Smartphones", "ModelId", "dbo.Models");
            DropForeignKey("dbo.Smartphones", "ColorTypeId", "dbo.ColorTypes");
            DropForeignKey("dbo.Smartphones", "BuiltInMemoryId", "dbo.BuiltInMemories");
            DropForeignKey("dbo.Lines", "BrandId", "dbo.Brands");
            DropIndex("dbo.Smartphones", new[] { "CasingMaterial_Id" });
            DropIndex("dbo.Smartphones", new[] { "BuiltInMemoryId" });
            DropIndex("dbo.Smartphones", new[] { "RAMSizeId" });
            DropIndex("dbo.Smartphones", new[] { "VersionTypeId" });
            DropIndex("dbo.Smartphones", new[] { "ColorTypeId" });
            DropIndex("dbo.Smartphones", new[] { "ModelId" });
            DropIndex("dbo.Models", new[] { "TypeOfMatrix_Id" });
            DropIndex("dbo.Models", new[] { "VersionType_Id" });
            DropIndex("dbo.Models", new[] { "CasingMaterialId" });
            DropIndex("dbo.Models", new[] { "ScreenResolutionId" });
            DropIndex("dbo.Models", new[] { "ProcessorId" });
            DropIndex("dbo.Models", new[] { "OSId" });
            DropIndex("dbo.Models", new[] { "ScreenRefreshRateId" });
            DropIndex("dbo.Models", new[] { "ScreenResolutionFormatId" });
            DropIndex("dbo.Models", new[] { "LineId" });
            DropIndex("dbo.Lines", new[] { "BrandId" });
            DropTable("dbo.TypeOfMatrices");
            DropTable("dbo.ScreenResolutionFormats");
            DropTable("dbo.ScreenResolutions");
            DropTable("dbo.ScreenRefreshRates");
            DropTable("dbo.Processors");
            DropTable("dbo.OS");
            DropTable("dbo.VersionTypes");
            DropTable("dbo.RAMs");
            DropTable("dbo.ColorTypes");
            DropTable("dbo.BuiltInMemories");
            DropTable("dbo.Smartphones");
            DropTable("dbo.CasingMaterials");
            DropTable("dbo.Models");
            DropTable("dbo.Lines");
            DropTable("dbo.Brands");
            CreateIndex("dbo.Products", "SubsectionId");
            CreateIndex("dbo.Subsections", "ParentSubsectionId");
            CreateIndex("dbo.Subsections", "CatalogId");
            AddForeignKey("dbo.Products", "SubsectionId", "dbo.Subsections", "Id");
            AddForeignKey("dbo.Subsections", "ParentSubsectionId", "dbo.Subsections", "Id");
            AddForeignKey("dbo.Subsections", "CatalogId", "dbo.Catalogs", "Id", cascadeDelete: true);
        }
    }
}
