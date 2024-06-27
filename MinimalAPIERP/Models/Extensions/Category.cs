using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ERP;

public partial class Category
{
    public Guid CategoryGuid { get; set; } = Guid.NewGuid();

}