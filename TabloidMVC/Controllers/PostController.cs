using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using System;
using TabloidMVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult MyPosts()
        {
            int userId = GetCurrentUserProfileId();
            var posts = _postRepository.GetUserPostById(userId);

            return View(posts);
        }

        
        public IActionResult Details(int id)
        {
            var post = new Post();
                post = _postRepository.GetPublishedPostById(id);
           
                if (post == null)
                {
                    return NotFound();
                }
                else
                {
                    post.TagNames = _postRepository.GetAllPostTagsByPostId(id);
                    return View(post);
                }
            
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }


        public ActionResult Edit(int id)
        {
            //making a new post
            var post = new Post();
            //getting current user
            int userId = GetCurrentUserProfileId();
            //getting the post  by user
            post = _postRepository.GetUserPostById(id, userId);
            //list of categories
            var categories = _categoryRepository.GetAll();
            //reusing the post create view model
            PostCreateViewModel vm = new PostCreateViewModel
            {
                //this is popluating the form 
                Post = post,
                //populating the categories
                CategoryOptions = categories
            };
            //if post is null or if the user profile dosen't match then you can edit
            if (post == null || post.UserProfileId != int.Parse(User.Claims.ElementAt(0).Value))
            {
                return NotFound();
            }
            else
            {
                //otherwise it will return the view model
                return View(vm);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Post post)
        {
            try
            {
                int userId = GetCurrentUserProfileId();
                _postRepository.update(post);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                var postToEdit = _postRepository.GetPublishedPostById(id);
                var categories = _categoryRepository.GetAll();
                PostCreateViewModel vm = new PostCreateViewModel
                {
                    Post = postToEdit,
                    CategoryOptions = categories
                };
                return View(vm);
            }
        }



        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }


        //Tag Management for the deatils of posts

        //GET
        public IActionResult TagManagement(int id)
        {
            var vm = new PostTagViewModel();
            vm.TagOptions = _tagRepository.GetAllTags();
            vm.Post = _postRepository.GetUserPostById(id, GetCurrentUserProfileId());

            if (vm.Post == null || vm.Post.UserProfileId != GetCurrentUserProfileId())
            {
                return NotFound();
            }
            
             return View(vm);                  
        }

        //POST
        [HttpPost]
        public IActionResult TagManagement(PostTagViewModel vm)
        {
            var postTag = new PostTag()
            {
                PostId = vm.Post.Id,
                TagId = vm.SelectedTagId
            };
            _postRepository.AddPostTag(postTag);
            return RedirectToAction("Index");
            
        }
        public ActionResult DeleteTag(int id)
        {
            List<PostTag> postTags = _postRepository.GetAllPostTagsByPostId(id);
            List<int> selectedPostTags = new List<int>();
            var vm = new DeletePostTagViewModel()
            {
                postTags = postTags,
                selectedPostTags = new List<int>()
            };
        
             
            return View(vm);
        }

        // POST: TagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTag(DeletePostTagViewModel vm, int id)
        {
                _postRepository.RemovePostTag(id);
            return View(RedirectToAction("Index"));


        }
    }
}
