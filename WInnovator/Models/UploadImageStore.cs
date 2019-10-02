using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WInnovator.Models
{
    public class UploadImageStore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("DesignShop")]
        public Guid DesignShopId { get; set; }
        public byte[] UploadedImage { get; set; }
        public string Mimetype { get; set; }

        public virtual DesignShop DesignShop { get; set; }

        public UploadImageStore()
        {
            Id = Guid.NewGuid();
        }
    }
}
