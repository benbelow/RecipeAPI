namespace RecipeAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStoreCupboardIngredientTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoreCupboardIngredients",
                c => new
                    {
                        StoreCupboardIngredientID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        IngredientID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StoreCupboardIngredientID)
                .ForeignKey("dbo.Ingredients", t => t.IngredientID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.IngredientID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreCupboardIngredients", "UserID", "dbo.Users");
            DropForeignKey("dbo.StoreCupboardIngredients", "IngredientID", "dbo.Ingredients");
            DropIndex("dbo.StoreCupboardIngredients", new[] { "IngredientID" });
            DropIndex("dbo.StoreCupboardIngredients", new[] { "UserID" });
            DropTable("dbo.StoreCupboardIngredients");
        }
    }
}
