namespace CookeryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRelationshipEventAndTicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ticket", "IdEvent", c => c.Int(nullable: false));
            AddColumn("dbo.Ticket", "Event_Id", c => c.Int());
            AlterColumn("dbo.Ticket", "FName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Ticket", "LName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Ticket", "Email", c => c.String(nullable: false, maxLength: 200));
            CreateIndex("dbo.Ticket", "Event_Id");
            AddForeignKey("dbo.Ticket", "Event_Id", "dbo.Event", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ticket", "Event_Id", "dbo.Event");
            DropIndex("dbo.Ticket", new[] { "Event_Id" });
            AlterColumn("dbo.Ticket", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Ticket", "LName", c => c.String());
            AlterColumn("dbo.Ticket", "FName", c => c.String());
            DropColumn("dbo.Ticket", "Event_Id");
            DropColumn("dbo.Ticket", "IdEvent");
        }
    }
}
