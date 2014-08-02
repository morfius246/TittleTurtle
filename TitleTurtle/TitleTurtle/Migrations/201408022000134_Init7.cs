namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacts", "ContactWebPage", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contacts", "ContactWebPage", c => c.String(nullable: false));
        }
    }
}
