using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ERP;

[Index("ProductId", Name = "IX_ProductId")]
[Index("CartId", Name = "IX_CartId")]
public partial class CartItem
{
    [Key]
    public int CartItemId { get; set; }

    public int ProductId { get; set; }
    
    public int CartId { get; set; }

    public int Count { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateCreated { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("CartItems")]
    public virtual Product Product { get; set; } = null!;
    [ForeignKey("CartId")]
    [InverseProperty("CartItems")]
    public virtual Cart Cart { get; set; } = null!;
}