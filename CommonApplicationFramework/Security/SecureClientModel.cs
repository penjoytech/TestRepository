//-----------------------------------------------------------------------
// <copyright file="SecureClientModel.cs" company="ASI">
//     Copyright (c) ASI . All rights reserved.
// </copyright>
// <author>Debabrata</author>
// <createdon>07-01-2015</createdon>
// <comment></comment>
//-----------------------------------------------------------------------

namespace CommonApplicationFramework.Security
{
    #region Namespaces
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Runtime.Serialization;  
    #endregion

    /// -----------------------------------------------------------------
    /// Namespace:      <ServtrackerModels>
    /// Class:          <SecureClientModel>
    /// Description:    <Description>
    /// Author:         <Debabrata>                    
    /// -----------------------------------------------------------------
    public class SecureClientModel
    {
        private readonly PasswordHasher _PasswordHasher = new PasswordHasher();
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Temporary_Code { get; set; }
        public string Salt { get; set; }
        public string UnhashedPassword { get; set; }

        public SecureClientModel(long userId, string UnHashedPassword)
        {
            this.Id = userId;
            this.SetSaltAndPassword(UnHashedPassword);
        }   
     
        private void SetSaltAndPassword(string password)
        {
            string salt;
            string hashedPassword;
            this._PasswordHasher.HashPassword(password, out hashedPassword, out salt);
            this.Salt = salt;
            this.Password = hashedPassword;
        }       
    }

    //public class UserModel
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string FirstName { get; set; }
    //    public string Password { get; set; }
    //    public string Status { get; set; }
    //    public string Email { get; set; }
    //    public string Salt { get; set; }
    //    public bool? IsFirstLogin { get; set; }
    //    public string CompanyCode { get; set; }
    //    public string UnhashedPassword { get; set; }
    //    //private readonly PasswordHasher _PasswordHasher = new PasswordHasher();

    //    //private void SetSaltAndPassword(string password)
    //    //{
    //    //    string salt;
    //    //    string hashedPassword;
    //    //    this._PasswordHasher.HashPassword(password, out hashedPassword, out salt);
    //    //    this.Salt = salt;
    //    //    this.Password = hashedPassword;
    //    //}
    //}
}
