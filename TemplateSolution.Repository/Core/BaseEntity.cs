using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TemplateSolution.Repository.Core
{
    public abstract class BaseEntity
    {
        [Browsable(false)]
        public int Id { get; set; }

        public BaseEntity()
        {
            
        }
    }
}
