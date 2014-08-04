namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init23 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacts", "ContactMobile", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contacts", "ContactMobile", c => c.String(maxLength: 9));
        }
    }
}
