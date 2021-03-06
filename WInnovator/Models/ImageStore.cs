﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WInnovator.Models
{
    [ExcludeFromCodeCoverage]
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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime UploadDateTime { get; set; }

        public virtual DesignShopWorkingForm DesignShopWorkingForm { get; set; }
    }
}
