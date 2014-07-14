namespace Blog1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Articles", "CategorieID", "dbo.Categories");
            DropIndex("dbo.Articles", new[] { "CategorieID" });
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        CategoryDescription = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            AddColumn("dbo.Articles", "CategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Articles", "CategoryID");
            AddForeignKey("dbo.Articles", "CategoryID", "dbo.Categories", "CategoryID", cascadeDelete: true);
            DropColumn("dbo.Articles", "CategorieID");
            DropTable("dbo.Categories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategorieID = c.Int(nullable: false, identity: true),
                        CategorieName = c.String(),
                        CategorieDescription = c.String(),
                    })
                .PrimaryKey(t => t.CategorieID);
            
            AddColumn("dbo.Articles", "CategorieID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Articles", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Articles", new[] { "CategoryID" });
            DropColumn("dbo.Articles", "CategoryID");
            DropTable("dbo.Categories");
            CreateIndex("dbo.Articles", "CategorieID");
            AddForeignKey("dbo.Articles", "CategorieID", "dbo.Categories", "CategorieID", cascadeDelete: true);
        }
    }
}
