namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProfile", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.Articles", "UserID", "dbo.Users");
            DropForeignKey("dbo.Contacts", "UserID", "dbo.Users");
            DropForeignKey("dbo.PersonalDatas", "UserID", "dbo.Users");
            DropForeignKey("dbo.Roles", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserPhotoes", "UserID", "dbo.Users");
            DropIndex("dbo.UserProfile", new[] { "User_UserID" });
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "UserID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Users", "UserID");
            AddForeignKey("dbo.Articles", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
            AddForeignKey("dbo.Contacts", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
            AddForeignKey("dbo.PersonalDatas", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
            AddForeignKey("dbo.Roles", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
            AddForeignKey("dbo.UserPhotoes", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
            DropTable("dbo.UserProfile");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        User_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.UserId);
            
            DropForeignKey("dbo.UserPhotoes", "UserID", "dbo.Users");
            DropForeignKey("dbo.Roles", "UserID", "dbo.Users");
            DropForeignKey("dbo.PersonalDatas", "UserID", "dbo.Users");
            DropForeignKey("dbo.Contacts", "UserID", "dbo.Users");
            DropForeignKey("dbo.Articles", "UserID", "dbo.Users");
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "UserID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Users", "UserID");
            CreateIndex("dbo.UserProfile", "User_UserID");
            AddForeignKey("dbo.UserPhotoes", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
            AddForeignKey("dbo.Roles", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
            AddForeignKey("dbo.PersonalDatas", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
            AddForeignKey("dbo.Contacts", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
            AddForeignKey("dbo.Articles", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
            AddForeignKey("dbo.UserProfile", "User_UserID", "dbo.Users", "UserID");
        }
    }
}
