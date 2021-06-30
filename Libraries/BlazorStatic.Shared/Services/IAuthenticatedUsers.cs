using System.Collections.Generic;
using System.Security.Claims;

namespace BlazorStatic.Shared.Services
{
    public interface IAuthenticatedUsers
    {
        void Add(ClaimsPrincipal user);
        void Remove(ClaimsPrincipal user);

        IEnumerable<ClaimsPrincipal> Users { get; }
    }

    public class AuthenticatedUsers : IAuthenticatedUsers
    {
        IList<ClaimsPrincipal> authenticatedUsers = new List<ClaimsPrincipal>();

        public AuthenticatedUsers()
        {
        }

        public IEnumerable<ClaimsPrincipal> Users => authenticatedUsers;

        public void Add(ClaimsPrincipal user)
        {
            if (!authenticatedUsers.Contains(user))
                authenticatedUsers.Add(user);
        }

        public void Remove(ClaimsPrincipal user)
        {
            if (authenticatedUsers.Contains(user))
                authenticatedUsers.Remove(user);
        }
    }
}
