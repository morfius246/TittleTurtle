namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Login", c => c.String(nullable: false, maxLength: 15));
            DropColumn("dbo.Users", "UserName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 15));
            DropColumn("dbo.Users", "Login");
        }
    }
}
