using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SocialNetDemo.Models;

namespace SocialNetDemo.Controllers
{
    [Authorize]
    public class FriendController : Controller
    {
        private ApplicationDbContext userDb = new ApplicationDbContext();

        // GET: Friend
        public ActionResult Index()
        {
            string id = User.Identity.GetUserId();
            List<ApplicationUser> friends = new List<ApplicationUser>();
            try
            {
                friends = userDb.Users.FirstOrDefault(u => u.Id.Equals(id)).Friends.ToList();
                friends.AddRange(userDb.Users.FirstOrDefault(u => u.Id.Equals(id)).FriendOf.ToList());
                foreach (ApplicationUser friend in friends)
                {
                    if(friend.ProfilePic==null) friend.ProfilePic=new byte[]{};
                }
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

            return View(friends);
        }


        public ActionResult SearchPeople()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchPeople(string name)
        {
            List<ApplicationUser> peoples;
            try
            {
                string id = User.Identity.GetUserId();
                List<ApplicationUser> friends = new List<ApplicationUser>();
                friends = userDb.Users.FirstOrDefault(u => u.Id.Equals(id)).Friends.ToList();
                friends.AddRange(userDb.Users.FirstOrDefault(u => u.Id.Equals(id)).FriendOf.ToList());
                peoples = userDb.Users.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();
                peoples = peoples.Except(friends).ToList();
                peoples = peoples.Except(userDb.Users.Where(u => u.Id.Equals(id))).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return View("ListView", peoples);
        }

        public ActionResult ListView(List<ApplicationUser> people)
        {
            return View(people);
        }

        public ActionResult ProfileView(string id)
        {
            try
            {
                ApplicationUser getUser = userDb.Users.FirstOrDefault(u => u.Id.Equals(id));
                return View(getUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View("Error");
            }
        }

        public ActionResult FriendProfileView(string id)
        {
            try
            {
                ApplicationUser getUser = userDb.Users.FirstOrDefault(u => u.Id.Equals(id));
                if(getUser.ProfilePic==null) getUser.ProfilePic=new byte[]{};
                if(getUser.CoverPic==null) getUser.CoverPic=new byte[]{};
                return View(getUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View("Error");
            }
        }

        public ActionResult AddFriend(string id)
        {
            try
            {
                string thisUserId = User.Identity.GetUserId();
                ApplicationUser aUser = userDb.Users.FirstOrDefault(u => u.Id.Equals(id));
                ApplicationUser thisUser = userDb.Users.FirstOrDefault(u => u.Id.Equals(thisUserId));

                thisUser.SentRequests.Add(aUser);

                userDb.Entry(thisUser).State=EntityState.Modified;
                userDb.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction("Index");
        }

        public ActionResult SeeRequests()
        {
            string thisUserId = User.Identity.GetUserId();
            ApplicationUser thisUser = userDb.Users.FirstOrDefault(u => u.Id.Equals(thisUserId));
            return View(thisUser.ReceivedRequests);
        }

        public ActionResult AcceptRequest(string id)
        {
            string thisUserId = User.Identity.GetUserId();
            ApplicationUser thisUser = userDb.Users.FirstOrDefault(u => u.Id.Equals(thisUserId));
            ApplicationUser aUser = userDb.Users.FirstOrDefault(u => u.Id.Equals(id));
            thisUser.Friends.Add(aUser);
            thisUser.ReceivedRequests.Remove(aUser);
            userDb.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RejectRequest(string id)
        {
            string thisUserId = User.Identity.GetUserId();
            ApplicationUser thisUser = userDb.Users.FirstOrDefault(u => u.Id.Equals(thisUserId));
            ApplicationUser aUser = userDb.Users.FirstOrDefault(u => u.Id.Equals(id));
            thisUser.ReceivedRequests.Remove(aUser);
            userDb.SaveChanges();
            return RedirectToAction("SeeRequests");
        }

        public ActionResult RemoveFriend(string id)
        {
            string thisUserId = User.Identity.GetUserId();
            ApplicationUser thisUser = userDb.Users.FirstOrDefault(u => u.Id.Equals(thisUserId));
            ApplicationUser aUser = userDb.Users.FirstOrDefault(u => u.Id.Equals(id));
            thisUser.Friends.Remove(aUser);
            thisUser.FriendOf.Remove(aUser);
            userDb.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
