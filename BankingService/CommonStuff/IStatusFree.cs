using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CommonStuff
{
    [ServiceContract]
    public interface IStatusFree
    {
        [OperationContract]
        bool IsItFree();
        
    }
}
