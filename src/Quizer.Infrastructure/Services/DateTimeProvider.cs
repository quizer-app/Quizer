using Quizer.Application.Common.Interfaces.Services;

namespace Quizer.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
