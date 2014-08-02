namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Edits", "Deletion", c => c.DateTime(nullable: false));
            DropColumn("dbo.Edits", "Delition");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Edits", "Delition", c => c.DateTime(nullable: false));
            DropColumn("dbo.Edits", "Deletion");
        }
    }
}
