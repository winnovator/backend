using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WInnovator.Models
{
    [ExcludeFromCodeCoverage]
    public class WorkingForm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required(ErrorMessage ="Geef een omschrijving van de werkvorm op")]
        [Display(Name ="Omschrijving")]
        public string Description { get; set; }
        [Required]
        [Range(1, 300, ErrorMessage = "Geef een waarde in minuten op, minimaal 1 minuut en maximaal 300 minuten")]
        [Display(Name = "Standaard tijd benodigd")]
        public int DefaultTimeNeeded { get; set; } = 1;
        [DataType(DataType.Html)]
        [Display(Name = "Inhoud")]
        public string? Content { get; set; }
        public Guid? belongsToDesignShopId;

        public virtual ICollection<DesignShopWorkingForm>? DesignShopWorkingForms { get; set; }
    }
}