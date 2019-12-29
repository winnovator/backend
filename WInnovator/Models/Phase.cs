using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WInnovator.Models
{
    [ExcludeFromCodeCoverage]
    public class Phase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Geef de naam van de fase op")]
        [Display(Name = "Fasenaam")]
        public string Name { get; set; }

        public virtual ICollection<WorkingForm>? WorkingForms { get; set; }
        public virtual ICollection<DesignShopWorkingForm>? DesignShopWorkingForms { get; set; }
    }
}
