using System;
using FollwUp.API.Enums;

namespace FollwUp.API.Helpers;

public static class IconNameMapper
{
    public static readonly Dictionary<IconType, Dictionary<string, string>> IconMap = new()
    {
        [IconType.Ionicons] = new Dictionary<string, string>
        {
            { "build-outline", "IoHomeOutline" },
            { "cash-outline", "IoSettingsOutline" },
            // Add more
        },
        [IconType.MaterialIcons] = new Dictionary<string, string>
        {
            { "home", "MdHome" },
            { "settings", "MdSettings" },
            // Add more
        }
    };
}
