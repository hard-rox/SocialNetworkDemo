using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SocialNetDemo.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string About { get; set; }
        [DisplayName("Area of Interest")]
        public string Interest { get; set; }
        [DisplayName("Living City")]
        public string City { get; set; }
        [DisplayName("Living Country")]
        public string Country { get; set; }
        [DisplayName("Life Experences")]
        public string Experience { get; set; }
        [DisplayName("Education Qualifications")]
        public string Education { get; set; }
        public string Gender { get; set; }
        [DisplayName("Profile Photo")]
        public byte[] ProfilePic { get; set; }
        [DisplayName("Cover Photo")]
        public byte[] CoverPic { get; set; }

        public virtual ICollection<ApplicationUser> Friends { get; set; }
        public virtual ICollection<ApplicationUser> FriendOf { get; set; }

        public virtual ICollection<ApplicationUser> ReceivedRequests { get; set; }
        public virtual ICollection<ApplicationUser> SentRequests{ get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("SocialCon", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasMany(m => m.Friends).WithMany(p => p.FriendOf).Map(
                w => w.ToTable("FriendList").MapLeftKey("UserId").MapRightKey("FriendId")
            );

            modelBuilder.Entity<ApplicationUser>().HasMany(m => m.SentRequests)
                .WithMany(p => p.ReceivedRequests)
                .Map(w => w.ToTable("Requests")
                    .MapLeftKey("SenderId").MapRightKey("ReceiverId"));

            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<SocialNetDemo.Models.Post> Posts { get; set; }

        public System.Data.Entity.DbSet<SocialNetDemo.Models.Comment> Comments { get; set; }
//public System.Data.Entity.DbSet<SocialNetDemo.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}