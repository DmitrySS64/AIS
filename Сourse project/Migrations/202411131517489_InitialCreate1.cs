namespace Сourse_project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Catalogs", "Url", c => c.String());
            AddColumn("dbo.Sections", "Url", c => c.String());
            AddColumn("dbo.Subsections", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subsections", "Url");
            DropColumn("dbo.Sections", "Url");
            DropColumn("dbo.Catalogs", "Url");
        }
    }
}
