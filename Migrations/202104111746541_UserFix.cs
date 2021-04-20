namespace LibrARRRy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Readers", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "Name", c => c.String());
            AddColumn("dbo.Users", "Surname", c => c.String());
            AddColumn("dbo.Workers", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Readers", "UserId");
            CreateIndex("dbo.Workers", "UserId");
            AddForeignKey("dbo.Readers", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Workers", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
            DropColumn("dbo.Readers", "Name");
            DropColumn("dbo.Readers", "Surname");
            DropColumn("dbo.Workers", "Name");
            DropColumn("dbo.Workers", "Surname");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Workers", "Surname", c => c.String());
            AddColumn("dbo.Workers", "Name", c => c.String());
            AddColumn("dbo.Readers", "Surname", c => c.String());
            AddColumn("dbo.Readers", "Name", c => c.String());
            DropForeignKey("dbo.Workers", "UserId", "dbo.Users");
            DropForeignKey("dbo.Readers", "UserId", "dbo.Users");
            DropIndex("dbo.Workers", new[] { "UserId" });
            DropIndex("dbo.Readers", new[] { "UserId" });
            DropColumn("dbo.Workers", "UserId");
            DropColumn("dbo.Users", "Surname");
            DropColumn("dbo.Users", "Name");
            DropColumn("dbo.Readers", "UserId");
        }
    }
}
