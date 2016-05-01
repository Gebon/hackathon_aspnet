namespace DepenedcyInjection.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(maxLength: 1000),
                        Character_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Characters", t => t.Character_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Character_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "Character_Id", "dbo.Characters");
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropIndex("dbo.Comments", new[] { "Character_Id" });
            DropTable("dbo.Comments");
        }
    }
}
