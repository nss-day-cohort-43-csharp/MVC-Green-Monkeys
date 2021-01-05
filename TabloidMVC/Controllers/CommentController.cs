using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IPostRepository _postRepo;
        public CommentController(ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _commentRepo = commentRepository;
            _postRepo = postRepository;
        }
        // GET: CommentController
        public ActionResult Index(int id)
        {
            List<Comment> comments = _commentRepo.GetCommentByPostId(id);
            Post post = _postRepo.GetPublishedPostById(id);
            CommentCreateViewModel vm = new CommentCreateViewModel
            {
                Comments = comments,
                Post = post
            };

            return View(vm);
        }

        // GET: CommentController/Details/5
        public ActionResult Details(int id)
        {       
            return View();
        }

        // GET: CommentController/Create
        public ActionResult Create(int id)
        {
            var currentUser = GetCurrentUserId();
            List<Comment> comments = _commentRepo.GetCommentByPostId(id);
            Post post = _postRepo.GetPublishedPostById(id);

            CommentCreateViewModel vm = new CommentCreateViewModel
            {
                UserId = currentUser,
                Comments = comments,
                Post = post
            };
            return View(vm);
        }

        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommentCreateViewModel vm)
        {
            try
            {
                vm.Comment.CreateDataTime = DateTime.Now;
                _commentRepo.AddComment(vm.Comment);
                return RedirectToAction("Index", new { id = vm.Comment.PostId });
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: CommentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CommentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
