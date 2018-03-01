namespace CookeryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTypePrice : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Event", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Event", "Price", c => c.Long(nullable: false));
        }
    }
}
