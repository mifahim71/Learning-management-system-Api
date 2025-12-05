using System.Security.Claims;

namespace LearningManagementSystemApi.Services
{
    public interface IJwtService
    {

        string GenerateToken(IEnumerable<Claim> claims);
    }
}
