using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Model
{
    [MessageContract]
    public class CommonResponse
    {
        [MessageHeader]
        public bool IsSuccessed;

        [MessageHeader]
        public string ResultMessage;
    }
}
