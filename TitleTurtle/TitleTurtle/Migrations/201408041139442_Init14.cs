namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init14 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Articles", "ArticleText", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Articles", "ArticleText", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
