namespace LibrARRRy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoanChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Loans", "ReaderId", "dbo.Readers");
            RenameColumn(table: "dbo.Loans", name: "ApplicationUser_Id", newName: "Reader_Id");
            RenameIndex(table: "dbo.Loans", name: "IX_ApplicationUser_Id", newName: "IX_Reader_Id");
            AddForeignKey("dbo.Loans", "Reader_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Loans", "Reader_Id", "dbo.AspNetUsers");
            RenameIndex(table: "dbo.Loans", name: "IX_Reader_Id", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Loans", name: "Reader_Id", newName: "ApplicationUser_Id");
            AddForeignKey("dbo.Loans", "ReaderId", "dbo.Readers", "ReaderId", cascadeDelete: true);
        }
    }
}
