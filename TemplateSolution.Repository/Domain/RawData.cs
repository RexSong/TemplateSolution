using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TemplateSolution.Repository.Core;

namespace TemplateSolution.Repository.Domain
{
    public class RawData : BaseEntity
    {
        [Key]
        public int id { get; set; }

        public string name { get; set; }

        public string desc { get; set; }

        public int age { get; set; }
    }
}
