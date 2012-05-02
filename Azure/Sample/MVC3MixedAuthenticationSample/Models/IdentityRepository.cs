using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC3MixedAuthenticationSample.Models
{
     
    public class IdentityRepository 
    {
        private ASPNETDBEntities _context;

        public IdentityRepository()
        {
            _context = new ASPNETDBEntities();
        }

        public int MapIdentity(string userid, string identytProvider, string identityValue)
        {
            var mapping = new UserIdentity();
            mapping.UserId = new Guid(userid);
            mapping.IdentityProvider = identytProvider;
            mapping.IdentityValue = identityValue;
            _context.AddToUserIdentities(mapping);
            _context.SaveChanges();

            return mapping.IdentityID;
        }

        public IQueryable<UserIdentity> FindByUserId(string userId)
        {
           return _context.UserIdentities.Where(i => i.UserId == new Guid(userId));
        }

        public UserIdentity FindByProvderAndValue(string IdentityProvider, string identityValue)
        {
            return _context.UserIdentities.FirstOrDefault(i => i.IdentityProvider == IdentityProvider && i.IdentityValue == identityValue);
        }

        public void DeleteById(int id, string userId)
        {
            var identity = _context.UserIdentities.FirstOrDefault(i => i.IdentityID == id && i.UserId == new Guid(userId));
            if (identity != null)
            {
                _context.DeleteObject(identity);
            }
            _context.SaveChanges();
        }

       
    }
}