namespace Blog1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Users", "UserID", c => c.Int(nullable: false, identity: false));
        }
        
        public override void Down()
        {
            AlterColumn("Users", "UserID", c => c.Int(nullable: false, identity: false));
        }
    }
}
