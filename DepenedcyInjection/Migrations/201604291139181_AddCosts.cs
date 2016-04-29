namespace DepenedcyInjection.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCosts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "Cost", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Characters", "Cost");
        }
    }
}
