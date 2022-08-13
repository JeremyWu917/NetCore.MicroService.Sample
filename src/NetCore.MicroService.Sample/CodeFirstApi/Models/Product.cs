using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirstApi.Models
{
    [Table("Product", Schema = "public")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Required]
        [Column(TypeName = "VARCHAR(16)")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 库存
        /// </summary>
        [Required]
        public int Stock { get; set; }
    }
}
