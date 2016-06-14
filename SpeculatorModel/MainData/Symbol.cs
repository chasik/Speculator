﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SpeculatorModel.MainData
{
    [DataContract]
    public class Symbol
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int LotSize { get; set; }

        [DataMember]
        public double? Punkt { get; set; }

        [DataMember]
        public double? Step { get; set; }
    }
}
