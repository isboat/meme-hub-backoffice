﻿using Meme.Domain.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace MemeTokenHub.Backoffce.Models
{
    public class UserModel: IModelItem
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public UserRoles? Role { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public enum UserRoles
    {
        //[Display(Name = "Editor")]
        Editor,

        //[Display(Name = "Admin")]
        Admin
    }
}
