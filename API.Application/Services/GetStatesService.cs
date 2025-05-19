using API.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Application.Services
{
    public class GetStatesService
    {
        public Dictionary<int, string> GetStates()
        {
            var values = Enum.GetValues(typeof(StateEnum))
                .Cast<StateEnum>()
                .ToDictionary(e => (int)e, e => e.ToString());

            return values;
        }
    }
}
