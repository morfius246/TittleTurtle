namespace Blog1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
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
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserFirstName = c.String(),
                        UserLastName = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        ArticleID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                        ArticleTitle = c.String(),
                        ArticleText = c.String(),
                        ArticleStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArticleID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        CategoryDescription = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Edits",
                c => new
                    {
                        EditID = c.Int(nullable: false, identity: true),
                        ArticleID = c.Int(nullable: false),
                        Creation = c.DateTime(nullable: false),
                        Edition = c.DateTime(nullable: false),
                        Delition = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EditID)
                .ForeignKey("dbo.Articles", t => t.ArticleID, cascadeDelete: true)
                .Index(t => t.ArticleID);
            
            CreateTable(
                "dbo.MediaInArticles",
                c => new
                    {
                        MediaInArticleID = c.Int(nullable: false, identity: true),
                        ArticleID = c.Int(nullable: false),
                        MediaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MediaInArticleID)
                .ForeignKey("dbo.Articles", t => t.ArticleID, cascadeDelete: true)
                .ForeignKey("dbo.Media", t => t.MediaID, cascadeDelete: true)
                .Index(t => t.ArticleID)
                .Index(t => t.MediaID);
            
            CreateTable(
                "dbo.Media",
                c => new
                    {
                        MediaID = c.Int(nullable: false, identity: true),
                        MediaData = c.Binary(),
                    })
                .PrimaryKey(t => t.MediaID);
            
            CreateTable(
                "dbo.UserPhotoes",
                c => new
                    {
                        UserPhotoID = c.Int(nullable: false, identity: true),
                        MediaID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        UserPhotoCurrent = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserPhotoID)
                .ForeignKey("dbo.Media", t => t.MediaID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.MediaID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        RatingID = c.Int(nullable: false, identity: true),
                        ArticleID = c.Int(nullable: false),
                        RatingLike = c.Int(nullable: false),
                        RatingDislike = c.Int(nullable: false),
                        RatingView = c.Int(nullable: false),
                        RatingRepost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RatingID)
                .ForeignKey("dbo.Articles", t => t.ArticleID, cascadeDelete: true)
                .Index(t => t.ArticleID);
            
            CreateTable(
                "dbo.TagInArticles",
                c => new
                    {
                        TagInArticleID = c.Int(nullable: false, identity: true),
                        ArticleID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TagInArticleID)
                .ForeignKey("dbo.Articles", t => t.ArticleID, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagID, cascadeDelete: true)
                .Index(t => t.ArticleID)
                .Index(t => t.TagID);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagID = c.Int(nullable: false, identity: true),
                        TagName = c.String(),
                    })
                .PrimaryKey(t => t.TagID);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ContactID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        ContactMobile = c.Int(nullable: false),
                        ContactEmail = c.String(),
                        ContactWebPage = c.String(),
                    })
                .PrimaryKey(t => t.ContactID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.PersonalDatas",
                c => new
                    {
                        PersonalDataID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        PersDataDate = c.DateTime(nullable: false),
                        PersDataAdress = c.String(),
                        PersDataOther = c.String(),
                    })
                .PrimaryKey(t => t.PersonalDataID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RolesID = c.String(nullable: false, maxLength: 128),
                        RoleName = c.String(),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RolesID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        MainArticleID = c.Int(),
                        ArticleID = c.Int(),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Articles", t => t.ArticleID)
                .ForeignKey("dbo.Articles", t => t.MainArticleID)
                .Index(t => t.MainArticleID)
                .Index(t => t.ArticleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "MainArticleID", "dbo.Articles");
            DropForeignKey("dbo.Comments", "ArticleID", "dbo.Articles");
            DropForeignKey("dbo.Accounts", "UserID", "dbo.Users");
            DropForeignKey("dbo.Roles", "UserID", "dbo.Users");
            DropForeignKey("dbo.PersonalDatas", "UserID", "dbo.Users");
            DropForeignKey("dbo.Contacts", "UserID", "dbo.Users");
            DropForeignKey("dbo.Articles", "UserID", "dbo.Users");
            DropForeignKey("dbo.TagInArticles", "TagID", "dbo.Tags");
            DropForeignKey("dbo.TagInArticles", "ArticleID", "dbo.Articles");
            DropForeignKey("dbo.Ratings", "ArticleID", "dbo.Articles");
            DropForeignKey("dbo.MediaInArticles", "MediaID", "dbo.Media");
            DropForeignKey("dbo.UserPhotoes", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserPhotoes", "MediaID", "dbo.Media");
            DropForeignKey("dbo.MediaInArticles", "ArticleID", "dbo.Articles");
            DropForeignKey("dbo.Edits", "ArticleID", "dbo.Articles");
            DropForeignKey("dbo.Articles", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Comments", new[] { "ArticleID" });
            DropIndex("dbo.Comments", new[] { "MainArticleID" });
            DropIndex("dbo.Roles", new[] { "UserID" });
            DropIndex("dbo.PersonalDatas", new[] { "UserID" });
            DropIndex("dbo.Contacts", new[] { "UserID" });
            DropIndex("dbo.TagInArticles", new[] { "TagID" });
            DropIndex("dbo.TagInArticles", new[] { "ArticleID" });
            DropIndex("dbo.Ratings", new[] { "ArticleID" });
            DropIndex("dbo.UserPhotoes", new[] { "UserID" });
            DropIndex("dbo.UserPhotoes", new[] { "MediaID" });
            DropIndex("dbo.MediaInArticles", new[] { "MediaID" });
            DropIndex("dbo.MediaInArticles", new[] { "ArticleID" });
            DropIndex("dbo.Edits", new[] { "ArticleID" });
            DropIndex("dbo.Articles", new[] { "CategoryID" });
            DropIndex("dbo.Articles", new[] { "UserID" });
            DropIndex("dbo.Accounts", new[] { "UserID" });
            DropTable("dbo.Comments");
            DropTable("dbo.Roles");
            DropTable("dbo.PersonalDatas");
            DropTable("dbo.Contacts");
            DropTable("dbo.Tags");
            DropTable("dbo.TagInArticles");
            DropTable("dbo.Ratings");
            DropTable("dbo.UserPhotoes");
            DropTable("dbo.Media");
            DropTable("dbo.MediaInArticles");
            DropTable("dbo.Edits");
            DropTable("dbo.Categories");
            DropTable("dbo.Articles");
            DropTable("dbo.Users");
            DropTable("dbo.Accounts");
        }
    }
}