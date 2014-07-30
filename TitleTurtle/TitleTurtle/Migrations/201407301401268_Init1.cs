namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Followers",
                c => new
                    {
                        FollowerID = c.Int(nullable: false, identity: true),
                        FollowID = c.Int(),
                        UserID = c.Int(),
                    })
                .PrimaryKey(t => t.FollowerID)
                .ForeignKey("dbo.Users", t => t.FollowID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.FollowID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Followers", "UserID", "dbo.Users");
            DropForeignKey("dbo.Followers", "FollowID", "dbo.Users");
            DropIndex("dbo.Followers", new[] { "UserID" });
            DropIndex("dbo.Followers", new[] { "FollowID" });
            DropTable("dbo.Followers");
        }
    }
}
