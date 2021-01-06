using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentCreateViewModel
    {
        public int UserId { get; set; }
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
        public Comment Comment { get; set; }
    }
}
