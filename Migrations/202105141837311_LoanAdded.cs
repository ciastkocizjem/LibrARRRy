namespace LibrARRRy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoanAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Loans",
                c => new
                    {
                        LoanId = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        ReaderId = c.Int(nullable: false),
                        LoanedDate = c.DateTime(nullable: false),
                        LoanExpireDate = c.DateTime(nullable: false),
                        ReturnedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.LoanId)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Readers", t => t.ReaderId, cascadeDelete: true)
                .Index(t => t.BookId)
                .Index(t => t.ReaderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Loans", "ReaderId", "dbo.Readers");
            DropForeignKey("dbo.Loans", "BookId", "dbo.Books");
            DropIndex("dbo.Loans", new[] { "ReaderId" });
            DropIndex("dbo.Loans", new[] { "BookId" });
            DropTable("dbo.Loans");
        }
    }
}
