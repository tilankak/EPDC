using CommonLib.Model;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLib.DataBase
{
    public class CustomUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserRoleStore<ApplicationUser>
    {
        private readonly IDbConnection _connection;
        public CustomUserStore(IDbConnection connection) => _connection = connection;

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            const string sql = "INSERT INTO [EPDC].[AspNetUsers] (Id, UserName, Email, PasswordHash) VALUES (@Id, @UserName, @Email, @PasswordHash)";
            await _connection.ExecuteAsync(sql, user);
            return IdentityResult.Success;
        }

        public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            const string sql = "SELECT * FROM [EPDC].[AspNetUsers] WHERE UPPER(UserName) = @UserName";
            return await _connection.QuerySingleOrDefaultAsync<ApplicationUser>(sql, new { UserName = normalizedUserName });
            
        }

        // Password support
        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
            => Task.FromResult(user.PasswordHash);

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
            => Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));

        // other methods (delete, update, findById) can be implemented similarly...
        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken) => throw new NotImplementedException();
       
        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken) => throw new NotImplementedException();
        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken) => Task.FromResult(user.Id);
        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) => Task.FromResult(user.UserName);
        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken) { user.UserName = userName; return Task.CompletedTask; }
        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken) => Task.FromResult(user.UserName.ToUpper());
        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken) => Task.CompletedTask;
        public void Dispose() { }

        //roles

        public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            var normalizedRoleName = roleName.ToUpperInvariant();
            const string roleSql = "SELECT Id FROM [EPDC].[AspNetRoles] WHERE NormalizedName = @normalizedRoleName";
            var roleId = await _connection.QueryFirstOrDefaultAsync<string>(roleSql, new { normalizedRoleName });

            if (roleId == null)
                throw new InvalidOperationException($"Role '{roleName}' not found.");

            const string userRoleSql = "INSERT INTO [EPDC].[AspNetUserRoles] (UserId, RoleId) VALUES (@UserId, @RoleId)";
            await _connection.ExecuteAsync(userRoleSql, new { UserId = user.Id, RoleId = roleId });
        }

        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            const string sql = @"
                DELETE FROM [EPDC].[AspNetUserRoles] 
                WHERE UserId=@UserId 
                AND RoleId=(SELECT Id FROM [EPDC].[AspNetRoles] WHERE NormalizedName=@normalizedRoleName)";
            await _connection.ExecuteAsync(sql, new { UserId = user.Id, normalizedRoleName = roleName.ToUpperInvariant() });
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT r.Name
                FROM [EPDC].[AspNetRoles] r
                INNER JOIN [EPDC].[AspNetUserRoles] ur ON r.Id = ur.RoleId
                WHERE ur.UserId = @UserId";
            var roles = await _connection.QueryAsync<string>(sql, new { UserId = user.Id });
            return roles.ToList();
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM [EPDC].[AspNetRoles] r
                INNER JOIN [EPDC].[AspNetUserRoles] ur ON r.Id = ur.RoleId
                WHERE ur.UserId = @UserId AND r.NormalizedName = @normalizedRoleName";
            var count = await _connection.ExecuteScalarAsync<int>(sql, new { UserId = user.Id, normalizedRoleName = roleName.ToUpperInvariant() });
            return count > 0;
        }

        public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            // optional — rarely used
            throw new NotImplementedException();
        }
    }
}
