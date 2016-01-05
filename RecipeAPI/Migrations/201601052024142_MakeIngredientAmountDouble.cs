namespace RecipeAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeIngredientAmountDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RecipeIngredients", "Amount", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RecipeIngredients", "Amount", c => c.Int(nullable: false));
        }
    }
}
