﻿using System.ComponentModel.DataAnnotations;

namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models
{
    public class Document
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Title field is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The Description field is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The FilePath field is required.")]
        public string FilePath { get; set; } // Changed from FileName to FilePath

        public DateTime UploadDate { get; set; }
    }
}