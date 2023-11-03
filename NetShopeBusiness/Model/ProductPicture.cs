using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetShopeBusiness.Model.Model
{
    public class ProductPicture
    {
        [Key]
        public int ProductPictureID { get; set; }

        [Display(Name = "Picture")]
        public string PicturePath { get; set; }

        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }
}
