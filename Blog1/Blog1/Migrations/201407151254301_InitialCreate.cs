namespace Blog1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "RoleName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "RoleName");
        }
    }
}
