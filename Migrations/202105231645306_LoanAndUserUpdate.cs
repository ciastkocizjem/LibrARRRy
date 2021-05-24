namespace LibrARRRy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoanAndUserUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CashPenalty", c => c.Boolean(nullable: false));
            AddColumn("dbo.Loans", "CollectionDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Loans", "CollectionDate");
            DropColumn("dbo.AspNetUsers", "CashPenalty");
        }
    }
}
