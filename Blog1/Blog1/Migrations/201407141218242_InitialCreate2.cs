namespace Blog1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "MainArticleID", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "MainArticleID");
            AddForeignKey("dbo.Comments", "MainArticleID", "dbo.Articles", "ArticleID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "MainArticleID", "dbo.Articles");
            DropIndex("dbo.Comments", new[] { "MainArticleID" });
            DropColumn("dbo.Comments", "MainArticleID");
        }
    }
}
