namespace Blog1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Autorizations",
                c => new
                    {
                        AutorizationID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Login = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.AutorizationID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Autorizations", "UserID", "dbo.Users");
            DropIndex("dbo.Autorizations", new[] { "UserID" });
            DropTable("dbo.Autorizations");
        }
    }
}
