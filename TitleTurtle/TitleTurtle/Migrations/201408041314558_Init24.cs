namespace TitleTurtle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init24 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PersonalDatas", "PersDataAdress", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PersonalDatas", "PersDataAdress", c => c.String(maxLength: 300));
        }
    }
}
