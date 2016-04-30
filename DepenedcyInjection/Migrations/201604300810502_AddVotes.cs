namespace DepenedcyInjection.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddVotes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VoteItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Position = c.Int(nullable: false),
                        Hero_Id = c.Int(),
                        Vote_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Hero_Id)
                .ForeignKey("dbo.Votes", t => t.Vote_Id)
                .Index(t => t.Hero_Id)
                .Index(t => t.Vote_Id);
            
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Week = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VoteItems", "Vote_Id", "dbo.Votes");
            DropForeignKey("dbo.Votes", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.VoteItems", "Hero_Id", "dbo.Characters");
            DropIndex("dbo.Votes", new[] { "User_Id" });
            DropIndex("dbo.VoteItems", new[] { "Vote_Id" });
            DropIndex("dbo.VoteItems", new[] { "Hero_Id" });
            DropTable("dbo.Votes");
            DropTable("dbo.VoteItems");
        }
    }
}
