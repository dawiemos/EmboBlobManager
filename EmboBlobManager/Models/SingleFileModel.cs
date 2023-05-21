using System.ComponentModel.DataAnnotations;


namespace EmboBlobManager.Models
{
    public class SingleFileModel
    {
        [Required(ErrorMessage = "Please enter file name")]
        public string FileName { get; set; }
        //[Required(ErrorMessage = "Please select file")]
        //public IFormFile File { get; set; }

    }
}
