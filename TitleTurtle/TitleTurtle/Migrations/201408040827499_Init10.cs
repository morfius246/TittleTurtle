namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Categories", "CategoryDescription", c => c.String(maxLength: 500));
            AlterColumn("dbo.Contacts", "ContactMobile", c => c.String(maxLength: 9));
            DropColumn("dbo.Users", "Login");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Login", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Contacts", "ContactMobile", c => c.String(nullable: false, maxLength: 9));
            AlterColumn("dbo.Categories", "CategoryDescription", c => c.String(nullable: false, maxLength: 500));
            DropColumn("dbo.Users", "UserName");
        }
    }
}
