namespace LibrARRRy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityFix : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "ConfimedInPanel");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ConfimedInPanel", c => c.Boolean(nullable: false));
        }
    }
}
