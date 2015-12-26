namespace RecipeAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Increasemaxinstructionlength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Instructions", "StepDescription", c => c.String(nullable: false, maxLength: 2000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Instructions", "StepDescription", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
