﻿namespace ELM.Application.DTOs;

public class BookDto
{
    public int BookId { get; set; }

    public string BookTitle { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string PublishDate { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string CoverBase64 { get; set; } = null!;
}