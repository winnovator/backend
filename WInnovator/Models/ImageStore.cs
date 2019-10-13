using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WInnovator.Models
{
    public class ImageStore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [ForeignKey("DesignShopWorkingForm")]
        public Guid DesignShopWorkingFormId { get; set; }
        [Required]
        public byte[] Image { get; set; }
        [Required]
        public string Mimetype { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]      // https://stackoverflow.com/questions/5252979/assign-format-of-datetime-with-data-annotations
        public DateTime UploadDateTime { get; set; }

        public virtual DesignShopWorkingForm DesignShopWorkingForm { get; set; }
    }
}
