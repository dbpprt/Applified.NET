using System.Threading.Tasks;

namespace Applified.Utilities.ApplifiedAdmin.Commands
{
    abstract class CommandBase
    {
        protected Options Options { get; private set; }

        public CommandBase(
            Options options
            )
        {
            Options = options;
        }

        public abstract Task<int> Execute();
    }
}
