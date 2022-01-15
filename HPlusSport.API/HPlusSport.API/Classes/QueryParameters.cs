﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace HPlusSport.API.Classes
{
    public class QueryParameters
    {
        const int _maxSize = 100;
        public int _size = 50;
        public int Page { get; set; }

        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = Math.Min(_maxSize, value);
            }
        }

    }
}
