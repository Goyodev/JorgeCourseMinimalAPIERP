using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ERP;

public partial class Store
{
    public Guid StoreGuid { get; set; } = Guid.NewGuid();

}
