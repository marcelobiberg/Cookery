namespace CookeryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChengedEntityEvnet : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Event", "Price", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Event", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
