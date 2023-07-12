using System.ComponentModel.DataAnnotations;

namespace VoTanPhatVCBRDemo.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}