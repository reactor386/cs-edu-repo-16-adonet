//-
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using EFPractices.Entities;


namespace EFPractices.Repositories;

public interface IBookRepository
{
    public List<Book> GetAll();
    public Book? GetById(int Id);
    public Book? GetByTitle(string title);
    public void Add(params Book[] item);
    public void Remove(params Book[] item);
    public void RemoveById(int Id);
    public void UpdateYearById(int Id, int year);

    public void MoveToUser(int Id, User? user);
    public List<Book> GetBorrowedBooks(string? user, string? title);
    public List<Book> FilterBooks(string? title, string? author, string? genre, int? yearL, int? yearU);
    public List<Book> GetBooksOrderedByYear(string? title, string? author, string? genre);
    public List<Book> GetBooksOrderedByTitle(string? title, string? author, string? genre);
}

public class BookRepository : IBookRepository
{
    public List<Book> GetAll()
    {
        List<Book> items;
        using (var db = new AppContext())
        {
            items = db.Books.ToList();
        }

        return items;
    }

    public Book? GetById(int Id)
    {
        Book? item;
        using (var db = new AppContext())
        {
            item = db.Books.Where(x => x.Id == Id).FirstOrDefault();
        }

        return item;
    }

    public Book? GetByTitle(string title)
    {
        Book? item;
        using (var db = new AppContext())
        {
            item = db.Books.Where(x => x.Title == title).FirstOrDefault();
        }

        return item;
    }

    public void Add(params Book[] item)
    {
        using (var db = new AppContext())
        {
            db.Books.AddRange(item);

            db.SaveChanges();
        }
    }

    public void Remove(params Book[] item)
    {
        using (var db = new AppContext())
        {
            db.Books.RemoveRange(item);

            db.SaveChanges();
        }
    }

    public void RemoveById(int Id)
    {
        using (var db = new AppContext())
        {
            db.Books.RemoveRange(db.Books.Where(x => x.Id == Id));

            db.SaveChanges();
        }
    }

    public void UpdateYearById(int Id, int year)
    {
        using (var db = new AppContext())
        {
            Book? item;
            item = db.Books.Where(x => x.Id == Id).FirstOrDefault();
            if (item is Book book)
            {
                book.Year = year;

                db.SaveChanges();
            }
        }
    }


    public void MoveToUser(int Id, User? user)
    {
        using (var db = new AppContext())
        {
            Book? item;
            item = db.Books.Where(x => x.Id == Id).FirstOrDefault();
            if (item is Book book)
            {
                // book.BorrowedByUser = user;
                book.UserId = user?.Id;

                db.SaveChanges();
            }
        }
    }

    public List<Book> GetBorrowedBooks(string? user, string? title)
    {
        List<Book> items;
        using (var db = new AppContext())
        {
            // дополняем список книг данными вложенных таблиц
            //  затем, фильтруем по полям полученного итогового запроса
            items = db.Books.Include(x => x.BorrowedByUser).Where(x =>
                (x.BorrowedByUser != null)
                && ((user != null) ? x.BorrowedByUser.Name == user : true)
                && ((title != null) ? x.Title == title : true)
            ).ToList();
        }

        return items;
    }

    public List<Book> FilterBooks(string? title, string? author, string? genre, int? yearL, int? yearU)
    {
        List<Book> items;
        using (var db = new AppContext())
        {
            // дополняем список книг данными вложенных таблиц
            //  затем, фильтруем по полям полученного итогового запроса
            items = db.Books.Include(x => x.Authors).ThenInclude(x => x.User).Include(x => x.Genre).Where(x =>
                ((title != null) ? x.Title == title : true)
                && ((author != null) ? x.Authors.Select(a => a.User.Name).Contains(author) : true)
                && ((genre != null) ? x.Genre.Name == genre : true)
                && ((yearL != null) ? x.Year >= yearL : true)
                && ((yearU != null) ? x.Year <= yearU : true)
            ).ToList();
        }

        return items;
    }

    public List<Book> GetBooksOrderedByYear(string? title, string? author, string? genre)
    {
        List<Book> items;
        using (var db = new AppContext())
        {
            // дополняем список книг данными вложенных таблиц
            //  затем, фильтруем по полям полученного итогового запроса
            items = db.Books.Include(x => x.Authors).ThenInclude(x => x.User).Include(x => x.Genre).Where(x =>
                ((title != null) ? x.Title == title : true)
                && ((author != null) ? x.Authors.Select(a => a.User.Name).Contains(author) : true)
                && ((genre != null) ? x.Genre.Name == genre : true)
            ).OrderByDescending(x => x.Year).ToList();
        }

        return items;
    }

    public List<Book> GetBooksOrderedByTitle(string? title, string? author, string? genre)
    {
        List<Book> items;
        using (var db = new AppContext())
        {
            // дополняем список книг данными вложенных таблиц
            //  затем, фильтруем по полям полученного итогового запроса
            items = db.Books.Include(x => x.Authors).ThenInclude(x => x.User).Include(x => x.Genre).Where(x =>
                ((title != null) ? x.Title == title : true)
                && ((author != null) ? x.Authors.Select(a => a.User.Name).Contains(author) : true)
                && ((genre != null) ? x.Genre.Name == genre : true)
            ).OrderBy(x => x.Title).ToList();
        }

        return items;
    }
}
