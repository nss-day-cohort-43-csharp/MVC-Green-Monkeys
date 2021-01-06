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
            return View(comments);
        }

        // GET: CommentController/Details/5
        public ActionResult Details(int id)
        {       
            return View();
        }

        // GET: CommentController/Create
        public ActionResult Create(int id)
        {
           
            Post post = _postRepo.GetPublishedPostById(id);
            CommentCreateViewModel vm = new CommentCreateViewModel()
            {
                Post = post,
                Comment = new Comment()
                {
                    PostId = post.Id,
                    UserProfileId = GetCurrentUserId()
                }
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
            Comment comment = _commentRepo.GetCommentById(id);
            if(comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Comment comment)
        {
            try
            {
                _commentRepo.UpdateComment(comment, id);
                return RedirectToAction(nameof(Index), new { id = comment.PostId});
            }
            catch(Exception ex)
            {
                return View(comment);
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
