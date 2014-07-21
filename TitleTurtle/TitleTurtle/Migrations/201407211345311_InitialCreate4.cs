namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "ArticleTittle", c => c.String());
            AddColumn("dbo.Edits", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Edits", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.Articles", "ArticleTitle");
            DropColumn("dbo.Edits", "Creation");
            DropColumn("dbo.Edits", "Edition");
            DropColumn("dbo.Edits", "Deletion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Edits", "Deletion", c => c.DateTime(nullable: false));
            AddColumn("dbo.Edits", "Edition", c => c.DateTime(nullable: false));
            AddColumn("dbo.Edits", "Creation", c => c.DateTime(nullable: false));
            AddColumn("dbo.Articles", "ArticleTitle", c => c.String());
            DropColumn("dbo.Edits", "Type");
            DropColumn("dbo.Edits", "Date");
            DropColumn("dbo.Articles", "ArticleTittle");
        }
    }
}
