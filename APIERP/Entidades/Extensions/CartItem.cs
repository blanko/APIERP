using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace APIERP.Entidades
{

    public partial class CartItem
    {
        public Guid CartItemGuid { get; set; }

    }
}
