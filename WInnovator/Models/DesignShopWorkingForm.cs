using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WInnovator.Models
{
    [ExcludeFromCodeCoverage]
    public class DesignShopWorkingForm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid DesignShopId { get; set; }
        public virtual DesignShop DesignShop { get; set; }
        [Required]
        public Guid WorkingFormId { get; set; }
        public virtual WorkingForm WorkingForm { get; set; }
        [Required]
        public int Order { get; set; }
        public bool IsCurrentWorkingForm { get; set; }

        public virtual ICollection<ImageStore> UploadedImages { get; set; }
    }
}