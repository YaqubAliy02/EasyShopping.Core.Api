﻿namespace Application.DTOs.Products
{
    public class ProductGetDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int StockQuantity { get; set; }
        public Guid CategoryId { get; set; }
        public string Thumbnail { get; set; }
    }
}