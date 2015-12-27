namespace RecipeAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adddescriptiontorecipeingredients : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RecipeIngredients", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RecipeIngredients", "Description");
        }
    }
}
