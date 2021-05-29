namespace LibrARRRy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SearchUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Search_SearchId", c => c.Int());
            CreateIndex("dbo.Books", "Search_SearchId");
            AddForeignKey("dbo.Books", "Search_SearchId", "dbo.Searches", "SearchId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "Search_SearchId", "dbo.Searches");
            DropIndex("dbo.Books", new[] { "Search_SearchId" });
            DropColumn("dbo.Books", "Search_SearchId");
        }
    }
}
