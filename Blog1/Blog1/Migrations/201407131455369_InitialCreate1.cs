namespace Blog1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
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
            CreateIndex("dbo.Articles", "CategorieID");
            AddForeignKey("dbo.Articles", "CategorieID", "dbo.Categories", "CategorieID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "CategorieID", "dbo.Categories");
            DropIndex("dbo.Articles", new[] { "CategorieID" });
            DropColumn("dbo.Articles", "CategorieID");
            DropTable("dbo.Categories");
        }
    }
}
