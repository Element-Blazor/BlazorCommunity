﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Community.DTO
{
   public class ChangePwdModel
    {
        public string Account { get; set; }
        public string OldPwd { get; set; }
        public string NewPwd { get; set; }
    }
}