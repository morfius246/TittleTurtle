namespace Blog1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "ArticleID", "dbo.Articles");
            DropIndex("dbo.Comments", new[] { "ArticleID" });
            AddColumn("dbo.Comments", "MainArticleID", c => c.Int());
            AlterColumn("dbo.Comments", "ArticleID", c => c.Int());
            CreateIndex("dbo.Comments", "MainArticleID");
            CreateIndex("dbo.Comments", "ArticleID");
            AddForeignKey("dbo.Comments", "MainArticleID", "dbo.Articles", "ArticleID");
            AddForeignKey("dbo.Comments", "ArticleID", "dbo.Articles", "ArticleID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "ArticleID", "dbo.Articles");
            DropForeignKey("dbo.Comments", "MainArticleID", "dbo.Articles");
            DropIndex("dbo.Comments", new[] { "ArticleID" });
            DropIndex("dbo.Comments", new[] { "MainArticleID" });
            AlterColumn("dbo.Comments", "ArticleID", c => c.Int(nullable: false));
            DropColumn("dbo.Comments", "MainArticleID");
            CreateIndex("dbo.Comments", "ArticleID");
            AddForeignKey("dbo.Comments", "ArticleID", "dbo.Articles", "ArticleID", cascadeDelete: true);
        }
    }
}
