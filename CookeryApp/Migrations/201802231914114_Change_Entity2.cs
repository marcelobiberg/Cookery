namespace CookeryApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Entity2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Event", "LongDescription", c => c.String(maxLength: 1200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Event", "LongDescription", c => c.String(maxLength: 800));
        }
    }
}
