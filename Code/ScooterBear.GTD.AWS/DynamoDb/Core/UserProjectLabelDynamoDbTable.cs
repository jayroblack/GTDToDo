﻿using System;
using Amazon.DynamoDBv2.DataModel;

namespace ScooterBear.GTD.AWS.DynamoDb.Core
{
    public static class UserProjectLabelTableData
    {
        public static readonly string User = "User";
        public static readonly string Project = "Project";
        public static readonly string Label = "Label";
        public static readonly string IndexProjectLabelByUserId = "ProjectLabelByUserId";
    }

    [DynamoDBTable("ToDo-UserProjectLabel")]
    public class UserProjectLabelDynamoDbTable : IDynamoDbTable
    {
        [DynamoDBHashKey] public string ID { get; set; }
        [DynamoDBRangeKey] public string Data { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string BillingId { get; set; }
        public string AuthId { get; set; }
        public bool IsAccountEnabled { get; set; }
        public string Name { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey("ProjectLabelByUserId")]
        public string UserId { get; set; }

        public DateTime DateCreated { get; set; }
        public int Count { get; set; }
        public bool IsDeleted { get; set; }
        public int CountOverDue { get; set; }
        [DynamoDBVersion] public int? VersionNumber { get; set; }
    }
}