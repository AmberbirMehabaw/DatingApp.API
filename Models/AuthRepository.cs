using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Models
{
    public class AuthRepository : IAuthRepository
    {
        public DataContext Context { get; }
        public AuthRepository(DataContext context)
        {
            this.Context = context;
            
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await Context.Users.FirstOrDefaultAsync(x=> x.Name == username);
            
            if(user == null)
                return null;

            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return (user);
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
              using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var ComputedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i=0; i<ComputedHash.Length; i++){
                    if(ComputedHash[i] != passwordHash[i])
                        return false;
                }
                return true;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte [] PasswordHash, PasswordSalt;
            CreatePassWordHash(password, out PasswordHash, out PasswordSalt);
            user.PasswordHash = PasswordHash;
            user.PasswordSalt = PasswordSalt;
            await Context.Users.AddAsync(user);
            await Context.SaveChangesAsync();
            return (user);
        }

        private void CreatePassWordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if(await Context.Users.AnyAsync(x=>x.Name == username))
                return true;
            
            return false;
        }
    }
}