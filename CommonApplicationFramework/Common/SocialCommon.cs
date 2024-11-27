using System;
using System.Collections.Generic;

namespace CommonApplicationFramework.Common
{
    public class DBInstance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DBServer { get; set; }
        public string DBName { get; set; }
        public string DBUserName { get; set; }
        public string DBPassword { get; set; }
        public string ConnectionId { get; set; }
    }

    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Guid Guid { get; set; }
        public string AuthMode { get; set; }
    }

    public class OrganizationUsers
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public List<Item> Users { get; set; }
    }

    public class URLs
    {
        public string Code { get; set; }
        public List<string> Url { get; set; }
    }


    public class SocialUserDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Provider { get; set; }
        public int CustomerId { get; set; }
        public string IdToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class UserDBInstance : DBInstance
    {
        public UserContext TanentContext { get; set; }
    }
}
