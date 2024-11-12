namespace Lab_6.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using static System.Data.Entity.Infrastructure.Design.Executor;

    internal sealed class Configuration : DbMigrationsConfiguration<Lab_7.ParseProductsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Lab_7.ParseProductsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            //Для обновления бд
            //PM > Add-Migration 
            //PM > Update-Database
        }
    }
}
