//-
using System;
using System.Collections.Generic;


namespace EFPractices.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int Year { get; set; }


    // Required one-to-many dependent (child)

    // Required foreign key property
    public int GenreId { get; set; }
    // Required reference navigation to principal
    public Genre Genre { get; set; } = null!;


    // Optional one-to-many dependent (child)

    // Optional foreign key property
    public int? UserId { get; set; }
    // Optional reference navigation to principal
    public User? BorrowedByUser { get; set; }


    // many-to-many

    // Collection navigation
    public List<Author> Authors { get; set; } = [];
}
