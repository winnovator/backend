using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WInnovator.Models
{
    public class DesignShop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "datetime2")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]      // https://stackoverflow.com/questions/5252979/assign-format-of-datetime-with-data-annotations
        public DateTime Date { get; set; }
        public Guid? CurrentDesignShopWorkingFormId { get; set; }

#nullable enable
        [ForeignKey("DesignShopWorkingForm")]
        public virtual DesignShopWorkingForm? CurrentDesignShopWorkingForm { get; set; }
#nullable restore
        public virtual ICollection<DesignShopWorkingForm> DesignShopWorkingForms { get; set; }
    }
}
