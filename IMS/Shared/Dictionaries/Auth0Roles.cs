using System.Collections.ObjectModel;

namespace Shared.Dictionaries;

public static class Auth0Roles
{
    public static readonly ReadOnlyDictionary<string, string> Roles = new(new Dictionary<string, string>()
    {
        { "HRManager", "rol_AkDRUefjwSgN6awP" },
        { "Mentor", "rol_Ch15B6uT1wbVIu6r" },
        { "Intern", "rol_6TnSKj1Big8liYkL" }
    });
}
