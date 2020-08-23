using NetCore.EntityFrameworkCore.EntityModels;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.EntityFrameworkCore.Models
{
    [Table("Test")]
    public class Test : BaseEntity<Guid>
    {
        public string Name { set; get; }
    }
}
