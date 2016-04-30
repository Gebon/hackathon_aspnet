namespace DepenedcyInjection.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class DeletePoints : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Points");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Points", c => c.Int(nullable: false));
        }
    }
}
