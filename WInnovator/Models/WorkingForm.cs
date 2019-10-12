using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WInnovator.Models
{
    public class WorkingForm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Description { get; set; }
        
        public virtual ICollection<DesignShopWorkingForm> DesignShopWorkingForms { get; set; }
    }
}