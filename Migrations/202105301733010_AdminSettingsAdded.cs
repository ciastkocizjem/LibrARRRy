namespace LibrARRRy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminSettingsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminSettings",
                c => new
                    {
                        SettingId = c.Int(nullable: false, identity: true),
                        BorrowedBooksLimit = c.Int(nullable: false),
                        DetentionLimit = c.Int(nullable: false),
                        AdminInfo = c.String(nullable: false, maxLength: 200),
                        InfoAdditionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SettingId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AdminSettings");
        }
    }
}
