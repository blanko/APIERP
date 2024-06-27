namespace APIERP.DTOs
{
    public class RaincheckDTOCompleto
    {
        public string Name { get; set; } = null!;
        public int Count { get; set; }
        public double SalePrice { get; set; }
        public RaincheckStoreDto Store { get; set; }
        public RaincheckProductDto Product { get; set; }
    }

    public class RaincheckStoreDto
    {
        public string Name { get; set; } = null!;
    }

    public class RaincheckProductDto
    {
        public string Name { get; set; } = null!;
        public RaincheckCategoryDto Category { get; set; }
    }

    public class RaincheckCategoryDto
    {
        public string Name { get; set; } = null!;
    }
}
