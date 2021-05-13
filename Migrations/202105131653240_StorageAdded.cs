namespace LibrARRRy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StorageAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Storages",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        CurrentAmount = c.Int(nullable: false),
                        Book_BookId = c.Int(),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.Books", t => t.Book_BookId)
                .Index(t => t.Book_BookId);
            
            AddColumn("dbo.Books", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Storages", "Book_BookId", "dbo.Books");
            DropIndex("dbo.Storages", new[] { "Book_BookId" });
            DropColumn("dbo.Books", "Description");
            DropTable("dbo.Storages");
        }
    }
}
