namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "UserFirstName", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Users", "UserLastName", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Users", "Login", c => c.String(nullable: false, maxLength: 15));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Login", c => c.String());
            AlterColumn("dbo.Users", "UserLastName", c => c.String());
            AlterColumn("dbo.Users", "UserFirstName", c => c.String());
        }
    }
}
