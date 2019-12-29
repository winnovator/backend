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
        public virtual DesignShop? DesignShop { get; set; }
        [Required]
        [Display(Name = "Werkvorm")]
        public Guid WorkingFormId { get; set; }
        [Display(Name = "Werkvorm")]
        public virtual WorkingForm? WorkingForm { get; set; }
        [Required(ErrorMessage = "Geef de weergavenaam van de werkvorm in de app op")]
        [Display(Name = "Weergavenaam")]
        public string DisplayName { get; set; }
        [Required(ErrorMessage = "Voer de uitvoerder in")]
        [Display(Name = "Uitvoerder")]
        public string Implementer { get; set; }
        [Required]
        [Range(1, 300, ErrorMessage = "Geef een waarde in minuten op, minimaal 1 minuut en maximaal 300 minuten")]
        [Display(Name = "Geplande benodigde tijd")]
        public int TimeAllocated { get; set; } = 1;
        [Display(Name = "Fase")]
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
        [Required]
        [Display(Name = "Positie")]
        public int Order { get; set; }
        [Display(Name = "Huidige werkvorm")]
        public bool IsCurrentWorkingForm { get; set; } = false;

        public virtual ICollection<ImageStore>? UploadedImages { get; set; }
    }
}