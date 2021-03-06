﻿using System;
using ScooterBear.GTD.Application.Users;

namespace ScooterBear.GTD.AWS.DynamoDb.Users
{
    public class ReadonlyUser : IUser
    {
        internal ReadonlyUser(string id, string firstName, string lastName, string email, string billingId,
            string authId, bool isAccountEnabled, int versionNumber, DateTime dateCreated)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BillingId = billingId;
            AuthId = authId;
            IsAccountEnabled = isAccountEnabled;
            VersionNumber = versionNumber;
            DateCreated = dateCreated;
        }

        public string ID { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string BillingId { get; }
        public string AuthId { get; }
        public bool? IsAccountEnabled { get; }
        public int VersionNumber { get; }
        public DateTime DateCreated { get; }
    }
}