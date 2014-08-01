using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Applified.Utilities.ApplifiedAdmin
{
    class CommandCollection
    {
        private readonly Dictionary<Expression<Func<Options, bool>>, Type> _commandMappings; 

        public CommandCollection()
        {
            _commandMappings = new Dictionary<Expression<Func<Options, bool>>, Type>();
        }

        public void RegisterType(
            Expression<Func<Options, bool>> matchExpression,
            Type targetType
            )
        {
            _commandMappings.Add(matchExpression, targetType);   
        }

        public Type GetMatch(
            Options options
            )
        {
            return _commandMappings
                .Where(commandMapping => 
                    commandMapping.Key.Compile().Invoke(options))
                .Select(commandMapping => commandMapping.Value)
                .FirstOrDefault();
        }
    }
}
