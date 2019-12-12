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
        [Display(Name = "DesignShop")]
        public Guid DesignShopId { get; set; }
        [Display(Name = "DesignShop")]
        public virtual DesignShop DesignShop { get; set; }
        [Required]
        [Display(Name = "Werkvorm")]
        public Guid WorkingFormId { get; set; }
        [Display(Name = "Werkvorm")]
        public virtual WorkingForm WorkingForm { get; set; }
        [Required]
        [Display(Name = "Positie")]
        public int Order { get; set; }
        [Display(Name = "Huidige werkvorm")]
        public bool IsCurrentWorkingForm { get; set; }

        public virtual ICollection<ImageStore> UploadedImages { get; set; }
    }
}