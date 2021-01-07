using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class DeletePostTagViewModel
    {
        public int SelectedPostTagId { get; set; }
        public Tag SelectedTag { get; set; }
        public Post Post { get; set; }
        public List<PostTag> postTags { get; set; }
        public List<int> selectedPostTags { get; set; }

    }
}
