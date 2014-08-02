namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Articles", "ArticleTitle", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Articles", "ArticleText", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Categories", "CategoryName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Categories", "CategoryDescription", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Contacts", "ContactMobile", c => c.String(nullable: false, maxLength: 9));
            AlterColumn("dbo.Contacts", "ContactWebPage", c => c.String(nullable: false));
            AlterColumn("dbo.PersonalDatas", "PersDataAdress", c => c.String(maxLength: 300));
            AlterColumn("dbo.PersonalDatas", "PersDataOther", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PersonalDatas", "PersDataOther", c => c.String());
            AlterColumn("dbo.PersonalDatas", "PersDataAdress", c => c.String());
            AlterColumn("dbo.Contacts", "ContactWebPage", c => c.String());
            AlterColumn("dbo.Contacts", "ContactMobile", c => c.String());
            AlterColumn("dbo.Categories", "CategoryDescription", c => c.String());
            AlterColumn("dbo.Categories", "CategoryName", c => c.String());
            AlterColumn("dbo.Articles", "ArticleText", c => c.String());
            AlterColumn("dbo.Articles", "ArticleTitle", c => c.String());
        }
    }
}
