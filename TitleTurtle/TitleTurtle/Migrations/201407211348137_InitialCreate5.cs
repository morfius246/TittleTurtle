namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "ArticleTitle", c => c.String());
            DropColumn("dbo.Articles", "ArticleTittle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articles", "ArticleTittle", c => c.String());
            DropColumn("dbo.Articles", "ArticleTitle");
        }
    }
}
