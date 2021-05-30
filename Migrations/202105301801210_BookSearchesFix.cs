namespace LibrARRRy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookSearchesFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "Search_SearchId", "dbo.Searches");
            DropIndex("dbo.Books", new[] { "Search_SearchId" });
            CreateTable(
                "dbo.SearchBooks",
                c => new
                    {
                        Search_SearchId = c.Int(nullable: false),
                        Book_BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Search_SearchId, t.Book_BookId })
                .ForeignKey("dbo.Searches", t => t.Search_SearchId, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.Book_BookId, cascadeDelete: true)
                .Index(t => t.Search_SearchId)
                .Index(t => t.Book_BookId);
            
            DropColumn("dbo.Books", "Search_SearchId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "Search_SearchId", c => c.Int());
            DropForeignKey("dbo.SearchBooks", "Book_BookId", "dbo.Books");
            DropForeignKey("dbo.SearchBooks", "Search_SearchId", "dbo.Searches");
            DropIndex("dbo.SearchBooks", new[] { "Book_BookId" });
            DropIndex("dbo.SearchBooks", new[] { "Search_SearchId" });
            DropTable("dbo.SearchBooks");
            CreateIndex("dbo.Books", "Search_SearchId");
            AddForeignKey("dbo.Books", "Search_SearchId", "dbo.Searches", "SearchId");
        }
    }
}
