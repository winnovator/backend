using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WInnovator.Models
{
    public class DesignShopWorkingForm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [ForeignKey("DesignShop")]
        public Guid DesignShopId { get; set; }
        [Required]
        [ForeignKey("WorkingForm")]
        public Guid WorkingFormId { get; set; }
        [Required]
        public int Order { get; set; }
        
        public virtual DesignShop DesignShop { get; set; }
        public virtual WorkingForm WorkingForm { get; set; }
#nullable enable
        [ForeignKey("DesignShop")]
        public virtual DesignShop? IsCurrentWorkingForm { get; set; }
#nullable restore

        public virtual ICollection<ImageStore> UploadedImages { get; set; }
    }
}