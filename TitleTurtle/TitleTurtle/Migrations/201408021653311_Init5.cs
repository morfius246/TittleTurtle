namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Login", c => c.String(maxLength: 15));
            AlterColumn("dbo.Contacts", "ContactMobile", c => c.String(maxLength: 9));
            AlterColumn("dbo.Contacts", "ContactWebPage", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contacts", "ContactWebPage", c => c.String(nullable: false));
            AlterColumn("dbo.Contacts", "ContactMobile", c => c.String(nullable: false, maxLength: 9));
            AlterColumn("dbo.Users", "Login", c => c.String(nullable: false, maxLength: 15));
        }
    }
}
