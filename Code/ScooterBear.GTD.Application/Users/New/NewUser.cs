using System;

namespace ScooterBear.GTD.Application.Users.New
{
    public class NewUser
    {
        public string ID { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public bool IsAccountEnabled { get; }
        public DateTime DateCreated { get; }

        public NewUser(string id, string firstName, string lastName, string email, DateTime dateCreated)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException($"{nameof(id)} is required.");

            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentException($"{nameof(firstName)} is required.");

            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentException($"{nameof(lastName)} is required.");

            if (string.IsNullOrEmpty(email))
                throw new ArgumentException($"{nameof(email)} is required.");

            if (dateCreated == default(DateTime))
                throw new ArgumentException($"Date Created value {dateCreated} is not valid.");

            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DateCreated = dateCreated;
            IsAccountEnabled = false;
        }
    }
}
