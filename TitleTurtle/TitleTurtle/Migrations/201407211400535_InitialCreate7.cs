namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate7 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Articles", "ArticleTitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articles", "ArticleTitle", c => c.String());
        }
    }
}
