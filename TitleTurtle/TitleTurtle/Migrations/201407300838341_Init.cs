namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contacts", "ContactMobile", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contacts", "ContactMobile", c => c.Int(nullable: false));
        }
    }
}
