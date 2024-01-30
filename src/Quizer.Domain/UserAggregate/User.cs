using Microsoft.AspNetCore.Identity;

namespace Quizer.Domain.UserAggregate;

public class User : IdentityUser<Guid>
{
    public User() : base()
    {
        
    }
}
