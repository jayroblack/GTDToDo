﻿using System;

namespace ScooterBear.GTD.Abstractions.Users
{
    public class UserAggregateRoot : IUser
    {
        public string ID { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public bool IsEmailVerified { get; }
        public string BillingId { get; }
        public string AuthId { get; }
        public bool IsAccountEnabled { get; }
        public DateTime DateCreated { get; }

        public UserAggregateRoot(string id, string firstName, string lastName, string email, DateTime dateCreated)
        {
            if( string.IsNullOrEmpty(id))
                throw new ArgumentException($"{nameof(id)} is required.");

            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentException($"{nameof(firstName)} is required.");

            if (string.IsNullOrEmpty(lastName))
                throw new ArgumentException($"{nameof(lastName)} is required.");

            if (string.IsNullOrEmpty(email))
                throw new ArgumentException($"{nameof(email)} is required.");
            
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DateCreated = dateCreated;
            IsEmailVerified = false;
            IsAccountEnabled = false;
        }
    }
}
