using System.ComponentModel.DataAnnotations.Schema;

namespace ERP;

public partial class Cart
{
    public int CartId { get; set; }

    public int? UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [InverseProperty("Cart")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}

