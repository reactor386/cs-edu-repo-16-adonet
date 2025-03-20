//-
using System;
using System.Collections.Generic;
using System.Linq;

using EFPractices.Entities;
using EFPractices.Repositories;


namespace EFPractices.Services;

/// <summary>
/// Класс, содержащий функции обращения к содержимому базы
/// </summary>
public class LibraryService
{
    IUserRepository _userRepository;
    IBookRepository _bookRepository;

    public LibraryService(IUserRepository userRepository,
        IBookRepository bookRepository)
    {
        _userRepository = userRepository;
        _bookRepository = bookRepository;
    }


    /// <summary>
    /// Метод построения базы, если база существует, она будет заменена
    /// </summary>
    public void BuildDatabase()
    {
        // Создаем контекст для добавления данных
        using (var db = new AppContext())
        {
            // Пересоздаем базу
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // заполняем базу данными

            User user1 = new() { Name = "Arthur", Email = "arthur@gmail.com" };
            User user2 = new() { Name = "Douglas", Email = "douglas@gmail.com" };
            User user3 = new() { Name = "Gabriel" };
            User user4 = new() { Name = "Travis" };
            db.Users.AddRange(user1, user2, user3, user4);

            Author author1 = new() { User = user1 };
            Author author2 = new() { User = user3 };
            db.Authors.AddRange(author1, author2);

            Genre genre1 = new() { Name = "Fairytale" };
            Genre genre2 = new() { Name = "Fiction" };
            Genre genre3 = new() { Name = "Novel" };
            db.Genres.AddRange( genre1, genre2, genre3);

            Book book1 = new() { Title = "Book1", Year = 2021, Genre = genre1, Authors = [author1] };
            Book book2 = new() { Title = "Book2", Year = 2022, Genre = genre2, Authors = [author2] };
            Book book3 = new() { Title = "Book3", Year = 2023, Genre = genre2, Authors = [author1, author2] };
            db.Books.AddRange(book1, book2, book3);
            
            db.SaveChanges();
        }
    }


    /// <summary>
    /// Метод возвращения книги пользователем
    /// </summary>
    /// <param name="userName">имя пользователя</param>
    /// <param name="bookTitle">название книги</param>
    public void ReturnBook(string userName, params string[] bookTitle)
    {
        User? user;
        user = _userRepository.GetByName(userName);

        if (user is User userItem)
        {
            List<Book?> books = bookTitle.Select(x => _bookRepository.GetByTitle(x))
                .Where(x => x?.UserId == userItem.Id).ToList();

            foreach (Book? book in books)
            {
                if (book is Book bookItem)
                    _bookRepository.MoveToUser(bookItem.Id, null);
            }
        }
    }


    /// <summary>
    /// Метод получения книги пользователем на руки
    /// </summary>
    /// <param name="userName">имя пользователя</param>
    /// <param name="bookTitle">название книги</param>
    public void BorrowBook(string userName, params string[] bookTitle)
    {
        User? user;
        user = _userRepository.GetByName(userName);

        if (user is User userItem)
        {
            List<Book?> books = bookTitle.Select(x => _bookRepository.GetByTitle(x))
                .Where(x => x?.UserId == null).ToList();

            foreach (Book? book in books)
            {
                if (book is Book bookItem)
                    _bookRepository.MoveToUser(bookItem.Id, userItem);
            }
        }
    }


    /// <summary>
    /// Функция получения списка занятых книг
    /// Если указан один или несколько аргументов,
    ///  то получаемый список фильтруется соответственным образом
    /// </summary>
    /// <param name="user">имя пользователя</param>
    /// <param name="title">название книги</param>
    /// <returns>список книг</returns>
    public List<Book> GetBorrowedBooks(string? user = null, string? title = null)
    {
        List<Book> res = _bookRepository.GetBorrowedBooks(
            user: user, title: title);

        return res;
    }

    /// <summary>
    /// Функция получения списка книг определенного жанра
    /// Если заданы значения годов,
    ///  то получаемый список фильтруется соответственным образом
    /// </summary>
    /// <param name="genreName">название жанра книги</param>
    /// <param name="yearL">минимальный год выхода книги</param>
    /// <param name="yearU">максимальный год выхода книги</param>
    /// <returns>список книг</returns>
    public List<Book> GetBookByGenre(string genreName, int? yearL = null, int? yearU = null)
    {
        List<Book> res = _bookRepository.FilterBooks(
            title: null, author: null, genre: genreName, yearL: yearL, yearU: yearU);

        return res;
    }

    /// <summary>
    /// Функция получения списка книг определенного автора
    /// Если задано название книги,
    ///  то получаемый список фильтруется соответственным образом
    /// </summary>
    /// <param name="author">имя автора</param>
    /// <param name="title">название книги</param>
    /// <returns>список книг</returns>
    public List<Book> GetBookByAuthor(string author, string? title = null)
    {
        List<Book> res = _bookRepository.FilterBooks(
            title: title, author: author, genre: null, yearL: null, yearU: null);

        return res;
    }

    /// <summary>
    /// Функция получения списка книг, сортированного
    ///  по году выхода книги в обратном порядке
    /// </summary>
    /// <returns>список книг</returns>
    public List<Book> GetBooksOrderedByYear()
    {
        List<Book> res = _bookRepository.GetBooksOrderedByYear(
            title: null, author: null, genre: null);

        return res;
    }

    /// <summary>
    /// Функция получения списка книг, сортированного
    ///  по наименованию в алфавитном порядке
    /// </summary>
    /// <returns>список книг</returns>
    public List<Book> GetBooksOrderedByTitle()
    {
        List<Book> res = _bookRepository.GetBooksOrderedByTitle(
            title: null, author: null, genre: null);

        return res;
    }
}
