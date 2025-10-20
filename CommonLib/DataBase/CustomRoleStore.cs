using CommonLib.Model;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLib.DataBase
{

    public class CustomRoleStore : IRoleStore<AppRole>
    {
        private readonly IDbConnection _connection;

        public CustomRoleStore(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IdentityResult> CreateAsync(AppRole role, CancellationToken cancellationToken)
        {
            const string sql = "INSERT INTO [EPDC].[AspNetRoles] (Id, Name) VALUES (@Id, @Name)";
            await _connection.ExecuteAsync(sql, new { role.Id, role.Name });
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(AppRole role, CancellationToken cancellationToken)
        {
            const string sql = "UPDATE [EPDC].[AspNetRoles] SET Name = @Name WHERE Id = @Id";
            var rows = await _connection.ExecuteAsync(sql, new { role.Id, role.Name });
            return rows > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Role not found." });
            
        }

        public async Task<IdentityResult> DeleteAsync(AppRole role, CancellationToken cancellationToken)
        {
            const string sql = "DELETE FROM [EPDC].[AspNetRoles] WHERE Id = @Id";
            var rows = await _connection.ExecuteAsync(sql, new { role.Id });
            return rows > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Role not found." });

           
        }

        public async Task<AppRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            const string sql = "SELECT * FROM [EPDC].[AspNetRoles] WHERE Id = @Id";
            return  await _connection.QuerySingleOrDefaultAsync<AppRole>(sql, new { Id = roleId });
        }

        public async Task<AppRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            const string sql = "SELECT * FROM [EPDC].[AspNetRoles] WHERE UPPER(Name) = @Name";
            return await _connection.QuerySingleOrDefaultAsync<AppRole>(sql, new { Name = normalizedRoleName });
            
        }

        public Task<string> GetNormalizedRoleNameAsync(AppRole role, CancellationToken cancellationToken)
            => Task.FromResult(role.Name.ToUpper());

        public Task<string> GetRoleIdAsync(AppRole role, CancellationToken cancellationToken)
            => Task.FromResult(role.Id);

        public Task<string> GetRoleNameAsync(AppRole role, CancellationToken cancellationToken)
            => Task.FromResult(role.Name);

        public Task SetNormalizedRoleNameAsync(AppRole role, string normalizedName, CancellationToken cancellationToken)
        {
            // You can store normalized name if needed; skipping here.
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(AppRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // no unmanaged resources to dispose
        }
    }
}
