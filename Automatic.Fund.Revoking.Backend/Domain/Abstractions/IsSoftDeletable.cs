﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    internal interface IsSoftDeletable
    {
        public bool IsDeleted { get; set; }

    }
}
