﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Model
{
  public  class CategoryResponseMessage
    {
        [MessageHeader]
        public bool IsSuccessed;

        [MessageHeader]
        public string ResultMessage;

        [MessageBodyMember]
        public DataTable DataTable;
    }
}
