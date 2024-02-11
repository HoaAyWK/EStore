using System.Text;
using EStore.Application.Common.Interfaces.Services;

namespace EStore.Infrastructure.Services;

public sealed class OtpService : IOtpService
{
    public string GenerateOtp(int length = 6)
    {
        var random = new Random();
        var opt = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            opt.Append(random.Next(0, 9));
        }

        return opt.ToString();
    }
}
