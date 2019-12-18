using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WInnovator.Models
{
    [ExcludeFromCodeCoverage]
    public class DesignShop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required(ErrorMessage ="Geef een omschrijving op")]
        [Display(Name ="Omschrijving")]
        public string Description { get; set; }
        [Required(ErrorMessage ="Geef de datum op")]
        [Column(TypeName = "datetime2")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Datum")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage ="Geef de starttijd op")]
        [DataType(DataType.Time)]
        [Display(Name = "Starttijd")]
        public TimeSpan Starttime { get; set; }
        public string? AppUseraccount { get; set; }

        public virtual ICollection<DesignShopWorkingForm>? DesignShopWorkingForms { get; set; }
    }
}
