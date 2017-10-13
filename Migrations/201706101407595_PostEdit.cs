namespace SocialNetDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostEdit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Posts", "ApplicationUserId");
            AddForeignKey("dbo.Posts", "ApplicationUserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Posts", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "UserId", c => c.String());
            DropForeignKey("dbo.Posts", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Posts", new[] { "ApplicationUserId" });
            DropColumn("dbo.Posts", "ApplicationUserId");
        }
    }
}
