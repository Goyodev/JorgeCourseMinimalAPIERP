using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ERP;

public partial class Order
{
    public Guid OrderGuid { get; set; } = Guid.NewGuid();

}
