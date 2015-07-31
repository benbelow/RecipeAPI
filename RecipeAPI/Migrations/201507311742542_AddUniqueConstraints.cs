namespace RecipeAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueConstraints : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.DietaryRequirements", "Name", unique: true);
            CreateIndex("dbo.Ingredients", "Name", unique: true);
            CreateIndex("dbo.Equipments", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Equipments", new[] { "Name" });
            DropIndex("dbo.Ingredients", new[] { "Name" });
            DropIndex("dbo.DietaryRequirements", new[] { "Name" });
        }
    }
}
