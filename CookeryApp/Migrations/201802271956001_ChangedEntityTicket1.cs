namespace CookeryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedEntityTicket1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Ticket", new[] { "Event_Id" });
            AlterColumn("dbo.Ticket", "Event_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Ticket", "Event_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Ticket", new[] { "Event_Id" });
            AlterColumn("dbo.Ticket", "Event_Id", c => c.Int());
            CreateIndex("dbo.Ticket", "Event_Id");
        }
    }
}
