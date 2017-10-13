using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SocialNetDemo.Models;

namespace SocialNetDemo.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private ApplicationDbContext postDb = new ApplicationDbContext();
        // GET: Post
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Post post)
        {
            post.ApplicationUserId = User.Identity.GetUserId();
            try
            {
                postDb.Posts.Add(post);
                postDb.SaveChanges();
                return RedirectToAction("ViewAllPost");
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                return View();
            }
            
        }

        public ActionResult ViewAllPost()
        {
            string id = User.Identity.GetUserId();
            try
            {
                List<Post> posts = postDb.Posts.Where(p => p.ApplicationUserId.Equals(id)).ToList();
                return View(posts);
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(int id)
        {
            try
            {
                Post aPost = new Post() {Id = id};
                postDb.Posts.Attach(aPost);
                postDb.Posts.Remove(aPost);
                postDb.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction("ViewAllPost");
        }

        public ActionResult TimeLine()
        {
            List<Post> timeLinePosts = new List<Post>();

            try
            {
                string thisUserId = User.Identity.GetUserId();
                ApplicationUser thisUser = postDb.Users.FirstOrDefault(u => u.Id.Equals(thisUserId));
                var friends = thisUser.Friends.ToList();
                friends.AddRange(thisUser.FriendOf.ToList());
                foreach (ApplicationUser friend in friends)
                {
                    timeLinePosts.AddRange(postDb.Posts.Where(p=>p.ApplicationUserId.Equals(friend.Id)));
                }
                timeLinePosts.AddRange(postDb.Posts.Where(p=>p.ApplicationUserId.Equals(thisUserId)));
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                throw;
            }

            return View(timeLinePosts);
        }

        public ActionResult LikeComment(int id)
        {
            Post post = new Post();
            try
            {
                post = postDb.Posts.FirstOrDefault(p => p.Id.Equals(id));
                ViewBag.post = post;
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
                throw;
            }
            return RedirectToAction("")
        }
    }
}