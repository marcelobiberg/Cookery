namespace CookeryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPriceOnEvent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Event", "Price", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Event", "Price");
        }
    }
}
