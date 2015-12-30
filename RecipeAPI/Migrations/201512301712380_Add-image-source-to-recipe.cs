namespace RecipeAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addimagesourcetorecipe : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipes", "ImageSource", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipes", "ImageSource");
        }
    }
}
