namespace Prettifier.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrittiferTableAddedAgain : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PrettifiedNumbers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OrginalNumber = c.String(nullable: false),
                        PrettifiedNumber = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        PrettifiedCategory = c.String(nullable: false, maxLength: 50),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrettifiedNumbers", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.PrettifiedNumbers", new[] { "UserId" });
            DropTable("dbo.PrettifiedNumbers");
        }
    }
}
