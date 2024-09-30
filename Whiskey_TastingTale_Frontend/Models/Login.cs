using System.ComponentModel.DataAnnotations;

namespace Whiskey_TastingTale_Backend.Model
{
    public class LoginResult
    {
        public required string token {  get; set; }
    }
}
