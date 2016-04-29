namespace DepenedcyInjection.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserPoints : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Points", c => c.Int(nullable: false, defaultValue: 15));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Points");
        }
    }
}
