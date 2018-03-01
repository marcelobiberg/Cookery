namespace CookeryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedEntityTicket : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Ticket", "IdEvent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ticket", "IdEvent", c => c.Int(nullable: false));
        }
    }
}
