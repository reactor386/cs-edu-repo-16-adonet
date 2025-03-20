//-
using System;
using System.Collections.Generic;
using System.Linq;

using EFPractices.Entities;
using EFPractices.Repositories;
using EFPractices.Services;


namespace EFPractices.App;

internal class Program
{
    static LibraryService? libraryService;

    /// <summary>
    /// Создаем базу и выполняем последовательно задания
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        Console.WriteLine("Library");
        Console.WriteLine("---");

        IUserRepository userRepository = new UserRepository();
        IBookRepository bookRepository = new BookRepository();

        libraryService = new LibraryService(userRepository, bookRepository);

        libraryService.BuildDatabase();


        // пользователь берет книгу
        Console.WriteLine("Пользователь [Douglas] берет книгу [Book1] ");
        libraryService.BorrowBook("Douglas", "Book1");

        // пользователь возвращает книгу
        // Console.WriteLine("Пользователь [Douglas] возвращает книгу [Book1] ");
        // libraryService.ReturnBook("Douglas", "Book1");


        // получаем список книг определенного жанра и вышедших между определенными годами
        Console.WriteLine("-");
        Console.WriteLine("Список книг жанра [Fiction] вышедших между годами 2020 и 2024: ");
        List<Book> books1 = libraryService.GetBookByGenre("Fiction", 2020, 2024);
        foreach (Book book in books1)
        {
            Console.WriteLine(book.Title);
        }

        // получаем количество книг определенного автора в библиотеке
        Console.WriteLine("-");
        Console.WriteLine("Количество книг автора [Arthur] в библиотеке: ");
        List<Book> books2 = libraryService.GetBookByAuthor("Arthur");
        Console.WriteLine(books2.Count);

        // получаем количество книг определенного жанра в библиотеке
        Console.WriteLine("-");
        Console.WriteLine("Количество книг жанра [Fiction] в библиотеке: ");
        List<Book> books3 = libraryService.GetBookByGenre("Fiction");
        Console.WriteLine(books3.Count);

        // получаем булевый флаг о том, есть ли книга определенного автора и с определенным названием в библиотеке
        Console.WriteLine("-");
        Console.WriteLine("Есть ли книга автора [Arthur] с названием [Book3] в библиотеке: ");
        List<Book> books4 = libraryService.GetBookByAuthor("Arthur", "Book3");
        Console.WriteLine(books4.Count > 0);

        // получаем булевый флаг о том, есть ли определенная книга на руках у пользователя
        Console.WriteLine("-");
        Console.WriteLine("Взята ли книга [Book1] пользователем [Douglas]: ");
        List<Book> books5 = libraryService.GetBorrowedBooks("Douglas", "Book1");
        Console.WriteLine(books5.Count > 0);

        // получаем количество книг на руках у пользователя
        Console.WriteLine("-");
        Console.WriteLine("Количество книг, взятых пользователем [Douglas]: ");
        List<Book> books6 = libraryService.GetBorrowedBooks("Douglas");
        Console.WriteLine(books6.Count);

        // получаем последнюю вышедшую книгу
        Console.WriteLine("-");
        Console.WriteLine("Последняя вышедшая книга: ");
        List<Book> books7 = libraryService.GetBooksOrderedByYear();
        Console.WriteLine(books7.Count > 0 ? books7.First().Title : "none");

        // получаем список всех книг, отсортированных в алфавитном порядке по названию
        Console.WriteLine("-");
        Console.WriteLine("Список всех книг в алфавитном порядке по названию: ");
        List<Book> books8 = libraryService.GetBooksOrderedByTitle();
        foreach (Book book in books8)
        {
            Console.WriteLine(book.Title);
        }

        // получаем список всех книг, отсортированных в порядке убывания года их выхода
        Console.WriteLine("-");
        Console.WriteLine("Список всех книг в порядке убывания года их выхода: ");
        List<Book> books9 = libraryService.GetBooksOrderedByYear();
        foreach (Book book in books9)
        {
            Console.WriteLine(book.Title);
        }


        Console.WriteLine("---");
        Console.ReadKey();
    }
}
