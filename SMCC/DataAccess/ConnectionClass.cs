using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMCC.Models;

namespace SMCC.DataAccess
{
    public class ConnectionClass
    {
        public Entities Entity { get; set; }

        public ConnectionClass()
        {
            Entity = new Entities();
        }
    }
}