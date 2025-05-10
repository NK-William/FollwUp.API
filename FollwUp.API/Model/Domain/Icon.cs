using System;
using FollwUp.API.Enums;

namespace FollwUp.API.Model.Domain;

public class Icon
{
    public string? Name { get; set; }
    public IconType Type { get; set; }
}
