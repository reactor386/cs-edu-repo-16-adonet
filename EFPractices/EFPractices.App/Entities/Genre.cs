//-
using System;
using System.Collections.Generic;


namespace EFPractices.Entities;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;


    // Required one-to-many principal (parent)

    // Collection navigation containing dependents
    public List<Book> Books { get; set; } = [];
}
