namespace CookeryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Entity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Event", "ShortDescription", c => c.String(maxLength: 500));
            AlterColumn("dbo.Event", "LongDescription", c => c.String(maxLength: 800));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Event", "LongDescription", c => c.String(maxLength: 500));
            AlterColumn("dbo.Event", "ShortDescription", c => c.String(maxLength: 300));
        }
    }
}
