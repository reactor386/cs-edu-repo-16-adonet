//-
using System;
using System.Collections.Generic;


namespace EFPractices.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; }


    // Required one-to-one principal (parent)

    // Reference navigation to dependent
    public Author? Author { get; set; }


    // Optional one-to-many principal (parent)

    // Collection navigation containing dependents
    public List<Book> BorrowedBooks { get; set; } = [];
}
