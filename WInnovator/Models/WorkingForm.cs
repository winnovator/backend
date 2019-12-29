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
        [Required(ErrorMessage ="Geef een naam van de werkvorm op")]
        [Display(Name ="Werkvorm naam")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Geef de weergavenaam van de werkvorm in de app op")]
        [Display(Name = "Weergavenaam")]
        public string DisplayName { get; set; }
        [Required]
        [Range(1, 300, ErrorMessage = "Geef een waarde in minuten op, minimaal 1 minuut en maximaal 300 minuten")]
        [Display(Name = "Standaard tijd benodigd")]
        public int DefaultTimeNeeded { get; set; } = 1;
        [Display(Name="Standaard fase")]
        public Guid? PhaseId { get; set; }
        public virtual Phase? Phase { get; set; }
        [Display(Name = "Foto-upload toegestaan")]
        public bool UploadEnabled { get; set; }
        [DataType(DataType.Html)]
        [Display(Name = "Samenvatting")]
        public string? Resume { get; set; }
        [DataType(DataType.Html)]
        [Display(Name = "Beschrijving")]
        public string? Description { get; set; }
        public Guid? belongsToDesignShopId { get; set; }
        public virtual DesignShop? belongsToDesignShop { get; set; }

        public virtual ICollection<DesignShopWorkingForm>? DesignShopWorkingForms { get; set; }
    }
}