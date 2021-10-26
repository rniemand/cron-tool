using System.Threading.Tasks;

namespace CronTools.Common.Models
{
  public interface ICronJob
  {
    Task Run();
  }
}
