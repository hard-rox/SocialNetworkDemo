namespace SocialNetDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestMachanism : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        SenderId = c.String(nullable: false, maxLength: 128),
                        ReceiverId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SenderId, t.ReceiverId })
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .ForeignKey("dbo.AspNetUsers", t => t.ReceiverId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "ReceiverId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requests", "SenderId", "dbo.AspNetUsers");
            DropIndex("dbo.Requests", new[] { "ReceiverId" });
            DropIndex("dbo.Requests", new[] { "SenderId" });
            DropTable("dbo.Requests");
        }
    }
}
