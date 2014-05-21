using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Model
{
    public class DataTableResponse
    {
        [MessageHeader]
        public bool IsSuccessful;

        [MessageHeader]
        public string ResultMessage;

        [MessageBodyMember]
        public DataTable DataTable;
    }
}
