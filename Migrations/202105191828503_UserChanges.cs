namespace LibrARRRy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Loans", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Searches", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Books", "ApplicationUser_Id");
            CreateIndex("dbo.Loans", "ApplicationUser_Id");
            CreateIndex("dbo.Searches", "ApplicationUser_Id");
            AddForeignKey("dbo.Books", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Loans", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Searches", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Searches", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Loans", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Books", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Searches", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Loans", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Books", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Searches", "ApplicationUser_Id");
            DropColumn("dbo.Loans", "ApplicationUser_Id");
            DropColumn("dbo.Books", "ApplicationUser_Id");
        }
    }
}
