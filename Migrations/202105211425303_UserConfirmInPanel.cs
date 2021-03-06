namespace LibrARRRy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserConfirmInPanel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ConfimedInPanel", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ConfimedInPanel");
        }
    }
}
