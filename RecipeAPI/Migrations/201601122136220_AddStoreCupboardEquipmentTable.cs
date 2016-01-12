namespace RecipeAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStoreCupboardEquipmentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StoreCupboardEquipments",
                c => new
                    {
                        StoreCupboardEquipmentID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        EquipmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StoreCupboardEquipmentID)
                .ForeignKey("dbo.Equipments", t => t.EquipmentID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.EquipmentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StoreCupboardEquipments", "UserID", "dbo.Users");
            DropForeignKey("dbo.StoreCupboardEquipments", "EquipmentID", "dbo.Equipments");
            DropIndex("dbo.StoreCupboardEquipments", new[] { "EquipmentID" });
            DropIndex("dbo.StoreCupboardEquipments", new[] { "UserID" });
            DropTable("dbo.StoreCupboardEquipments");
        }
    }
}
