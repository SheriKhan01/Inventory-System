﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Dtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
