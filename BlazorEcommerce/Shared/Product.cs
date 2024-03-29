﻿namespace BlazorEcommerce.Shared;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string ImageUrl { get; set; } = String.Empty;
    public Category? Category { get; set; }
    public int? CategoryId { get; set; }
    public bool Featured { get; set; } = false;
    public List<ProductVariant> ProductVariants { get; set; } = new();
}