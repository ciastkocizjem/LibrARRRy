namespace LibrARRRy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoanChanges2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Loans", "ReaderId", "dbo.Readers");
            DropForeignKey("dbo.Loans", "Reader_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Loans", new[] { "ReaderId" });
            DropIndex("dbo.Loans", new[] { "Reader_Id" });
            DropColumn("dbo.Loans", "ReaderId");
            RenameColumn(table: "dbo.Loans", name: "Reader_Id", newName: "ReaderId");
            AddColumn("dbo.Loans", "Reader_ReaderId", c => c.Int());
            AlterColumn("dbo.Loans", "ReaderId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Loans", "ReaderId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Loans", "ReaderId");
            CreateIndex("dbo.Loans", "Reader_ReaderId");
            AddForeignKey("dbo.Loans", "Reader_ReaderId", "dbo.Readers", "ReaderId");
            AddForeignKey("dbo.Loans", "ReaderId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Loans", "ReaderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Loans", "Reader_ReaderId", "dbo.Readers");
            DropIndex("dbo.Loans", new[] { "Reader_ReaderId" });
            DropIndex("dbo.Loans", new[] { "ReaderId" });
            AlterColumn("dbo.Loans", "ReaderId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Loans", "ReaderId", c => c.Int(nullable: false));
            DropColumn("dbo.Loans", "Reader_ReaderId");
            RenameColumn(table: "dbo.Loans", name: "ReaderId", newName: "Reader_Id");
            AddColumn("dbo.Loans", "ReaderId", c => c.Int(nullable: false));
            CreateIndex("dbo.Loans", "Reader_Id");
            CreateIndex("dbo.Loans", "ReaderId");
            AddForeignKey("dbo.Loans", "Reader_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Loans", "ReaderId", "dbo.Readers", "ReaderId", cascadeDelete: true);
        }
    }
}
