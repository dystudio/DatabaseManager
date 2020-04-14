﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseConverter.Model
{
    public class FunctionSpecification
    {
        private string _delimiter;
        public string Name { get; set; }
        public string Args { get; set; }

        public string Delimiter
        {
            get
            {
                if (string.IsNullOrEmpty(this._delimiter))
                {
                    return ",";
                }

                return this._delimiter;
            }
            set
            {
                this._delimiter = value;
            }
        }
    }

    public enum FunctionArgumentToken
    {
        UNKNOWN = 0,
        TYPE = 1,
        EXP = 2, //Expression
        VALUE = 3,
        START = 4,
        LENGTH = 5
    }
}