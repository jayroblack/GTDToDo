using System;

namespace ScooterBear.GTD.Application.Users.New
{
    public class NewUser
    {
        public string ID { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string AuthId { get; }
        public bool IsEmailVerified { get; }
        public bool IsAccountEnabled { get; }
        public DateTime DateCreated { get; }

        public NewUser(string id, string firstName, string lastName, string email, DateTime dateCreated, string authId)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException($"{nameof(id)} is required.");

            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentException($"{nameof(firstName)} is required.");

            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentException($"{nameof(lastName)} is required.");

            if (string.IsNullOrEmpty(email))
                throw new ArgumentException($"{nameof(email)} is required.");

            if (string.IsNullOrEmpty(authId))
                throw new ArgumentException($"{nameof(authId)} is required.");

            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DateCreated = dateCreated;
            AuthId = authId;
            IsEmailVerified = false;
            IsAccountEnabled = false;
        }
    }
}
