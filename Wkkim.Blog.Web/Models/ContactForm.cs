using System.ComponentModel.DataAnnotations;

namespace Wkkim.Blog.Web.Models
{
    public class ContactForm
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Phone No is Required")]
        public string PhoneNo { get; set; }
        [Required(ErrorMessage = "Message is Required")]
        public string Message { get; set; }
    }
}
