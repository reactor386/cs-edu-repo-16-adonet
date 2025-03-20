//-
using System;
using System.Collections.Generic;


namespace EFPractices.Entities;

public class Author
{
    public int Id { get; set; }


    // Required one-to-one dependent (child)

    // Required foreign key property
    public int UserId { get; set; }
    // Required reference navigation to principal
    public User User { get; set; } = null!;


    // many-to-many

    // Collection navigation
    public List<Book> Books { get; set; } = [];
}
