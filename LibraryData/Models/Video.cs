using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibraryData.Models
{
    public class Video
    {
        [Required]
        public string Director { get; set; }
    }
}
