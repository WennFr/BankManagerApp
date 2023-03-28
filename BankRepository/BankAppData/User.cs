using System;
using System.Collections.Generic;

namespace BankRepository.BankAppData
{
    public partial class User
    {
        public int UserId { get; set; }
        public string LoginName { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
