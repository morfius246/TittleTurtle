namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "ArticleTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "ArticleTitle");
        }
    }
}
