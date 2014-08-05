namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init28 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        LikeID = c.Int(nullable: false, identity: true),
                        ArticleID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        Likes = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LikeID);
            
            AddColumn("dbo.Articles", "Like_LikeID", c => c.Int());
            AddColumn("dbo.Users", "Like_LikeID", c => c.Int());
            CreateIndex("dbo.Articles", "Like_LikeID");
            CreateIndex("dbo.Users", "Like_LikeID");
            AddForeignKey("dbo.Articles", "Like_LikeID", "dbo.Likes", "LikeID");
            AddForeignKey("dbo.Users", "Like_LikeID", "dbo.Likes", "LikeID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Like_LikeID", "dbo.Likes");
            DropForeignKey("dbo.Articles", "Like_LikeID", "dbo.Likes");
            DropIndex("dbo.Users", new[] { "Like_LikeID" });
            DropIndex("dbo.Articles", new[] { "Like_LikeID" });
            DropColumn("dbo.Users", "Like_LikeID");
            DropColumn("dbo.Articles", "Like_LikeID");
            DropTable("dbo.Likes");
        }
    }
}
