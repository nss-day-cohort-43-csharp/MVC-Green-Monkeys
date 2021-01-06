using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        List<Comment> GetCommentByPostId(int id);
        void AddComment(Comment comment);
        Comment GetCommentById(int id);
        void UpdateComment(Comment comment, int id);
    }
}
