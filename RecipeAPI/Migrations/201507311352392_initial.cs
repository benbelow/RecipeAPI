namespace RecipeAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DietaryRequirements",
                c => new
                    {
                        DietaryRequirementID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.DietaryRequirementID);
            
            CreateTable(
                "dbo.IngredientDietaryRequirements",
                c => new
                    {
                        IngredientDietaryRequirementID = c.Int(nullable: false, identity: true),
                        IngredientID = c.Int(),
                        DietaryRequirementID = c.Int(),
                    })
                .PrimaryKey(t => t.IngredientDietaryRequirementID)
                .ForeignKey("dbo.DietaryRequirements", t => t.DietaryRequirementID)
                .ForeignKey("dbo.Ingredients", t => t.IngredientID)
                .Index(t => t.IngredientID)
                .Index(t => t.DietaryRequirementID);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        IngredientID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 60),
                    })
                .PrimaryKey(t => t.IngredientID);
            
            CreateTable(
                "dbo.RecipeIngredients",
                c => new
                    {
                        RecipeIngredientID = c.Int(nullable: false, identity: true),
                        RecipeID = c.Int(),
                        IngredientID = c.Int(),
                        Amount = c.Int(nullable: false),
                        Units = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.RecipeIngredientID)
                .ForeignKey("dbo.Ingredients", t => t.IngredientID)
                .ForeignKey("dbo.Recipes", t => t.RecipeID)
                .Index(t => t.RecipeID)
                .Index(t => t.IngredientID);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        RecipeID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 500),
                        MealType = c.String(nullable: false, maxLength: 30),
                        PreparationTime = c.Int(nullable: false),
                        CookTime = c.Int(nullable: false),
                        NumberOfServings = c.Int(nullable: false),
                        Author = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RecipeID);
            
            CreateTable(
                "dbo.Instructions",
                c => new
                    {
                        InstructionID = c.Int(nullable: false, identity: true),
                        RecipeID = c.Int(),
                        StepNumber = c.Int(nullable: false),
                        StepDescription = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.InstructionID)
                .ForeignKey("dbo.Recipes", t => t.RecipeID)
                .Index(t => t.RecipeID);
            
            CreateTable(
                "dbo.RecipeEquipments",
                c => new
                    {
                        RecipeEquipmentID = c.Int(nullable: false, identity: true),
                        RecipeID = c.Int(),
                        EquipmentID = c.Int(),
                    })
                .PrimaryKey(t => t.RecipeEquipmentID)
                .ForeignKey("dbo.Equipments", t => t.EquipmentID)
                .ForeignKey("dbo.Recipes", t => t.RecipeID)
                .Index(t => t.RecipeID)
                .Index(t => t.EquipmentID);
            
            CreateTable(
                "dbo.Equipments",
                c => new
                    {
                        EquipmentID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.EquipmentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IngredientDietaryRequirements", "IngredientID", "dbo.Ingredients");
            DropForeignKey("dbo.RecipeIngredients", "RecipeID", "dbo.Recipes");
            DropForeignKey("dbo.RecipeEquipments", "RecipeID", "dbo.Recipes");
            DropForeignKey("dbo.RecipeEquipments", "EquipmentID", "dbo.Equipments");
            DropForeignKey("dbo.Instructions", "RecipeID", "dbo.Recipes");
            DropForeignKey("dbo.RecipeIngredients", "IngredientID", "dbo.Ingredients");
            DropForeignKey("dbo.IngredientDietaryRequirements", "DietaryRequirementID", "dbo.DietaryRequirements");
            DropIndex("dbo.RecipeEquipments", new[] { "EquipmentID" });
            DropIndex("dbo.RecipeEquipments", new[] { "RecipeID" });
            DropIndex("dbo.Instructions", new[] { "RecipeID" });
            DropIndex("dbo.RecipeIngredients", new[] { "IngredientID" });
            DropIndex("dbo.RecipeIngredients", new[] { "RecipeID" });
            DropIndex("dbo.IngredientDietaryRequirements", new[] { "DietaryRequirementID" });
            DropIndex("dbo.IngredientDietaryRequirements", new[] { "IngredientID" });
            DropTable("dbo.Equipments");
            DropTable("dbo.RecipeEquipments");
            DropTable("dbo.Instructions");
            DropTable("dbo.Recipes");
            DropTable("dbo.RecipeIngredients");
            DropTable("dbo.Ingredients");
            DropTable("dbo.IngredientDietaryRequirements");
            DropTable("dbo.DietaryRequirements");
        }
    }
}
