namespace RecipeAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Make_FK_relationships_of_instructions_required : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Instructions", "RecipeID", "dbo.Recipes");
            DropIndex("dbo.Instructions", new[] { "RecipeID" });
            AlterColumn("dbo.Instructions", "RecipeID", c => c.Int(nullable: false));
            CreateIndex("dbo.Instructions", "RecipeID");
            AddForeignKey("dbo.Instructions", "RecipeID", "dbo.Recipes", "RecipeID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Instructions", "RecipeID", "dbo.Recipes");
            DropIndex("dbo.Instructions", new[] { "RecipeID" });
            AlterColumn("dbo.Instructions", "RecipeID", c => c.Int());
            CreateIndex("dbo.Instructions", "RecipeID");
            AddForeignKey("dbo.Instructions", "RecipeID", "dbo.Recipes", "RecipeID");
        }
    }
}
