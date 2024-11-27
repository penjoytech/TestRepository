using System;
using System.Collections.Generic;

namespace CommonApplicationFramework.Common
{
    public class BaseModel
    {
        public DateTimeOffset? CreatedOn { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public string Creator { get; set; }

        public string Modifier { get; set; }

    }

    public class ItemCode : Item
    {
        public string Code { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }

        public string Value { get; set; }
    }

    public class ItemGUID 
    {
        public int Id { get; set; }

        public Guid GUID { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }


    public class UserModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public string Password { get; set; }

        public string Status { get; set; }

        public string Email { get; set; }

        public string Salt { get; set; }

        public bool? IsFirstLogin { get; set; }

        public string UnhashedPassword { get; set; }

        public DateTimeOffset ExpiresOn { get; set; }

        public string CompanyCode { get; set; }

        public string IsRestricted { get; set; }

        public bool IsSuperUser { get; set; }
        public string AuthenticationMode { get; set; }
        public string UserGuid { get; set; }
    }

    public class LogManagerModel : BaseModel
    {
        public int UserId { get; set; }

        public string Module { get; set; }

        public int Activity { get; set; }

        public string Message { get; set; }

        public string IPAddress { get; set; }
    }

    public class UserLoginModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool? IsFirstLogin { get; set; }

        public DateTimeOffset? ExpiresOn { get; set; }

        public string Status { get; set; }

        public string FileName { get; set; }

        public string CurCompanyCode { get; set; }

        public List<ItemCode> Module { get; set; }

        public string CurOrganisationName { get; set; }

        public Guid CurOrganisationGuid { get; set; }

        public bool IsSuperUser { get; set; }

        public bool IsGuestUser { get; set; }

        public Item Designation { get; set; }

        public List<CompanyModule> CompanyModules { get; set; }

        public bool IsAdmin { get; set; }
        public int ControlUserId { get; set; }
        public int ApplicationUserId { get; set; }
        public string LoginId { get; set; }
		public Item UserType { get; set; }
		public ItemCode Country { get; set; }
	}

    public class UserLogin
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string AgentCode { get; set; }

        public string grant_type { get; set; }
    }

    public class UserContext
    {
        public long UserId { get; set; }
        public Company Org { get; set; }
        public string CurCompanyCode { get; set; }

        public Guid CurOrganization { get; set; }

        public int CurOrganizationId { get; set; }

        public string CurOrganisationName { get; set; }

        public int CurProjectId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public bool? IsFirstLogin { get; set; }

        public bool? IsSuperAdmin { get; set; }

        public string IPAddress { get; set; }

        public string Status { get; set; }

        public DateTimeOffset? ExpireOn { get; set; }

        public string FileName { get; set; }

        public UserToken Token { get; set; }

        public List<ItemCode> CompanyList { get; set; }

        public List<Organization> OrganizationList { get; set; }

        public List<UserDBInstance> InstanceList { get; set; }

        public List<ItemCode> Modules { get; set; }

        public List<ItemCode> Actions { get; set; }
        public List<ItemCode> Resources { get; set; }

        public List<ItemCode> UserGroups { get; set; }

        public List<ItemCode> Roles { get; set; }

        public List<Item> Folders { get; set; }

        public int TokenDuration { get; set; }

        public DateTime TokenExpireOn { get; set; }

        public bool IsSuperUser { get; set; }

        public List<Item> UserType { get; set; }

        public int Cart { get; set; }

        public bool IsOTPVerified { get; set; }

        public bool IsGuest { get; set; }

        public Item Designation { get; set; }

        public bool IsAdmin { get; set; }

        public string UserGuid { get; set; }

		public ItemCode Country { get; set; }
        public ItemCode ApplicationType { get; set; }

    }

    public class AttachmentFile
    {
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets of Attachment FileName
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets of Attachment File
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// Gets or sets of Attachment FileType
        /// </summary>
        public string FileType { get; set; }

        public string MimeType { get; set; }
    }

    public class CompanyModel
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string Code { get; set; }

        public string DBServerName { get; set; }

        public string DBName { get; set; }

        public string DBUserName { get; set; }
        public string DBPassword { get; set; }
    }

    public class CompanyContext
    {
        public List<CompanyModel> Companies { get; set; }
    }

    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string AuthMode { get; set; }
        public string CompanyId { get; set; }
        public string Guid { get; set; }
        public string FileStorageProvider { get; set; }
        public string FileStorageSystem { get; set; }
    }

    public class CompanyDetails: Company
    {
        public string DomainName { get; set; }
        public string ActiveDirectoryURL { get; set; }
        public string ADUserId { get; set; }
        public string ADPassword { get; set; }
    }
    public class UserToken
    {
        public string Token { get; set; }
        public int Duration { get; set; }
        public DateTime ExpireOn { get; set; }
        public string Status { get; set; }
    }

    public class Token
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Value { get; set; }
        public int Duration { get; set; }
        public DateTime ExpireOn { get; set; }
        public string Status { get; set; }
        public string CompanyCode { get; set; }
        public string ModuleCode { get; set; }
        public string IPAddress { get; set; }
    }

    public class CustomerModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public string Email { get; set; }

        public string Salt { get; set; }

        public bool? IsFirstLogin { get; set; }

        public string UnhashedPassword { get; set; }

        public DateTimeOffset ExpiresOn { get; set; }

        public string CompanyCode { get; set; }

        public string IsRestricted { get; set; }

        public bool IsSuperUser { get; set; }

        public int GuestCart { get; set; }

        public bool IsOTPVerified { get; set; }

        public bool IsGuest { get; set; }

        public bool IsAdmin { get; set; }
    }

    public class RelationalModel
    {
        public int RelationalId { get; set; }

        public Item Items { get; set; }
    }

    public class CompanyModule : ApplicationTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ConnectionId { get; set; }
        public string AuthenticationMode { get; set; }
        public string CompanyModuleName { get; set; }
    }
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public UserContext UserContext { get; set; }
    }
      
    public class UserContextDetail
    {
        public UserToken Token { get; set; }
        public UserLoginModel UserBasicDetails { get; set; }
        public List<Module> Modules { get; set; }  
    }

    public class ApplicationTypeModel  
    {
        public string ApplicationType { get; set; }
        public string ApplicationTypeCode { get; set; }
        public string Logo { get; set; }
        public string Icon { get; set; }
    }

    public class PhoneModel
    {
        public int Id { get; set; }

        public int PhoneType { get; set; }
       
        public string PhoneNumber { get; set; }

        public int? Extn { get; set; }

        public string ContactType { get; set; }

        public int ContactId { get; set; }

        public int IsPrimary { get; set; }

        public string Status { get; set; }
    }

    public class PhoneTypeModel : BaseModel
    {

        public int Id { get; set; }

        public Item PhoneType { get; set; }

    }
}
