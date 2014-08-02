namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Roles", "UserID", "dbo.Users");
            DropIndex("dbo.Roles", new[] { "UserID" });
            DropTable("dbo.Roles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RolesID = c.String(nullable: false, maxLength: 128),
                        RoleName = c.String(),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RolesID);
            
            CreateIndex("dbo.Roles", "UserID");
            AddForeignKey("dbo.Roles", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
        }
    }
}
