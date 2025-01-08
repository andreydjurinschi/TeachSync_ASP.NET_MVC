using System.ComponentModel.DataAnnotations;

namespace TeachSyncApp.Models;

public enum RoleType
{
    Admin,
    Manager,
    Teacher
}

public class Role
{
 public int Id { get; set; }
 public RoleType Name { get; set; }
 public ICollection<User> Users { get; set; } = new List<User>();//одна роль у нескольких пользователей
 
}