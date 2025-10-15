using CommonLib.Model;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLib.DataBase
{
    public class CustomUserStore : IUserStore<AppUser>, IUserPasswordStore<AppUser>
    {
        private readonly IDbConnection _connection;
        public CustomUserStore(IDbConnection connection) => _connection = connection;

        public async Task<IdentityResult> CreateAsync(AppUser user, CancellationToken cancellationToken)
        {
            const string sql = "INSERT INTO Users (Id, UserName, Email, PasswordHash) VALUES (@Id, @UserName, @Email, @PasswordHash)";
            //await _connection.ExecuteAsync(sql, user);
            return IdentityResult.Success;
        }

        public async Task<AppUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            //const string sql = "SELECT * FROM Users WHERE UPPER(UserName) = @UserName";
            //return await _connection.QuerySingleOrDefaultAsync<AppUser>(sql, new { UserName = normalizedUserName });
            return new AppUser() { Email= "tilankak@gmail.com", Id="slsi", PasswordHash = "", UserName = "tilankak@gmail.com" };
        }

        // Password support
        public Task SetPasswordHashAsync(AppUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(AppUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.PasswordHash);

        public Task<bool> HasPasswordAsync(AppUser user, CancellationToken cancellationToken)
            => Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));

        // other methods (delete, update, findById) can be implemented similarly...
        public Task<IdentityResult> DeleteAsync(AppUser user, CancellationToken cancellationToken) => throw new NotImplementedException();
       
        public Task<IdentityResult> UpdateAsync(AppUser user, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task<AppUser?> FindByIdAsync(string userId, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task<string> GetUserIdAsync(AppUser user, CancellationToken cancellationToken) => Task.FromResult(user.Id);
        public Task<string> GetUserNameAsync(AppUser user, CancellationToken cancellationToken) => Task.FromResult(user.UserName);
        public Task SetUserNameAsync(AppUser user, string userName, CancellationToken cancellationToken) { user.UserName = userName; return Task.CompletedTask; }
        public Task<string> GetNormalizedUserNameAsync(AppUser user, CancellationToken cancellationToken) => Task.FromResult(user.UserName.ToUpper());
        public Task SetNormalizedUserNameAsync(AppUser user, string normalizedName, CancellationToken cancellationToken) => Task.CompletedTask;
        public void Dispose() { }
    }
}
