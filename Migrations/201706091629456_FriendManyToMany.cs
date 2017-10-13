namespace SocialNetDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FriendManyToMany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FriendList",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        FriendId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.FriendId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.FriendId)
                .Index(t => t.UserId)
                .Index(t => t.FriendId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FriendList", "FriendId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FriendList", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.FriendList", new[] { "FriendId" });
            DropIndex("dbo.FriendList", new[] { "UserId" });
            DropTable("dbo.FriendList");
        }
    }
}
