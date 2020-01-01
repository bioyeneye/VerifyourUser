using System;
using System.Collections.Generic;
using System.Text;

namespace VerifyourUser.DataAccess.Entities
{
    public class User
    {
        public User(string firstName, string lastName, string email, bool emailConfirmed, string phoneNumber, bool phoneNumberConfirmed)
        {
            Id = Guid.NewGuid().ToString();
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            EmailConfirmed = emailConfirmed;
            PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
            PhoneNumberConfirmed = phoneNumberConfirmed;
            CreatedAt = DateTime.UtcNow;
        }

        public string Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
        public Audience Audience { get; set; }
        public ICollection<UserVerificationToken> UserVerificationTokens { get; set; }
    }
}
