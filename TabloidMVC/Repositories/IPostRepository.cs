using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        void update(Post post);
        void Delete(int postId);
        List<Post> GetAllPublishedPosts();
        List<Post> GetUserPostById(int userProfileId);
        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);
        void AddPostTag(PostTag postTag);
        List<PostTag> GetAllPostTagsByPostId(int id);
        void RemovePostTag(int id);
    }
}