namespace RecipeAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Make_FK_relationships_of_recipe_required : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RecipeIngredients", "IngredientID", "dbo.Ingredients");
            DropForeignKey("dbo.RecipeIngredients", "RecipeID", "dbo.Recipes");
            DropForeignKey("dbo.RecipeEquipments", "RecipeID", "dbo.Recipes");
            DropForeignKey("dbo.RecipeEquipments", "EquipmentID", "dbo.Equipments");
            DropIndex("dbo.RecipeIngredients", new[] { "RecipeID" });
            DropIndex("dbo.RecipeIngredients", new[] { "IngredientID" });
            DropIndex("dbo.RecipeEquipments", new[] { "RecipeID" });
            DropIndex("dbo.RecipeEquipments", new[] { "EquipmentID" });
            AlterColumn("dbo.RecipeIngredients", "RecipeID", c => c.Int(nullable: false));
            AlterColumn("dbo.RecipeIngredients", "IngredientID", c => c.Int(nullable: false));
            AlterColumn("dbo.RecipeEquipments", "RecipeID", c => c.Int(nullable: false));
            AlterColumn("dbo.RecipeEquipments", "EquipmentID", c => c.Int(nullable: false));
            CreateIndex("dbo.RecipeIngredients", "RecipeID");
            CreateIndex("dbo.RecipeIngredients", "IngredientID");
            CreateIndex("dbo.RecipeEquipments", "RecipeID");
            CreateIndex("dbo.RecipeEquipments", "EquipmentID");
            AddForeignKey("dbo.RecipeIngredients", "IngredientID", "dbo.Ingredients", "IngredientID", cascadeDelete: true);
            AddForeignKey("dbo.RecipeIngredients", "RecipeID", "dbo.Recipes", "RecipeID", cascadeDelete: true);
            AddForeignKey("dbo.RecipeEquipments", "RecipeID", "dbo.Recipes", "RecipeID", cascadeDelete: true);
            AddForeignKey("dbo.RecipeEquipments", "EquipmentID", "dbo.Equipments", "EquipmentID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RecipeEquipments", "EquipmentID", "dbo.Equipments");
            DropForeignKey("dbo.RecipeEquipments", "RecipeID", "dbo.Recipes");
            DropForeignKey("dbo.RecipeIngredients", "RecipeID", "dbo.Recipes");
            DropForeignKey("dbo.RecipeIngredients", "IngredientID", "dbo.Ingredients");
            DropIndex("dbo.RecipeEquipments", new[] { "EquipmentID" });
            DropIndex("dbo.RecipeEquipments", new[] { "RecipeID" });
            DropIndex("dbo.RecipeIngredients", new[] { "IngredientID" });
            DropIndex("dbo.RecipeIngredients", new[] { "RecipeID" });
            AlterColumn("dbo.RecipeEquipments", "EquipmentID", c => c.Int());
            AlterColumn("dbo.RecipeEquipments", "RecipeID", c => c.Int());
            AlterColumn("dbo.RecipeIngredients", "IngredientID", c => c.Int());
            AlterColumn("dbo.RecipeIngredients", "RecipeID", c => c.Int());
            CreateIndex("dbo.RecipeEquipments", "EquipmentID");
            CreateIndex("dbo.RecipeEquipments", "RecipeID");
            CreateIndex("dbo.RecipeIngredients", "IngredientID");
            CreateIndex("dbo.RecipeIngredients", "RecipeID");
            AddForeignKey("dbo.RecipeEquipments", "EquipmentID", "dbo.Equipments", "EquipmentID");
            AddForeignKey("dbo.RecipeEquipments", "RecipeID", "dbo.Recipes", "RecipeID");
            AddForeignKey("dbo.RecipeIngredients", "RecipeID", "dbo.Recipes", "RecipeID");
            AddForeignKey("dbo.RecipeIngredients", "IngredientID", "dbo.Ingredients", "IngredientID");
        }
    }
}
