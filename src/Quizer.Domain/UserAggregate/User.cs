using Microsoft.AspNetCore.Identity;

namespace Quizer.Domain.UserAggregate
{
    public class User : IdentityUser<UserId>
    {
        public User() : base()
        {
            
        }
    }
}
