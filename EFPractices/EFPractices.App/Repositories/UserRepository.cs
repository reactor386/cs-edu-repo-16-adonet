//-
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using EFPractices.Entities;


namespace EFPractices.Repositories;

public interface IUserRepository
{
    public List<User> GetAll();
    public User? GetById(int Id);
    public User? GetByName(string name);
    public void Add(params User[] item);
    public void Remove(params User[] item);
    public void RemoveById(int Id);
    public void UpdateNameById(int Id, string name);
}

public class UserRepository : IUserRepository
{
    public List<User> GetAll()
    {
        List<User> items;
        using (var db = new AppContext())
        {
            items = db.Users.ToList();
        }

        return items;
    }

    public User? GetById(int Id)
    {
        User? item;
        using (var db = new AppContext())
        {
            item = db.Users.Where(x => x.Id == Id).FirstOrDefault();
        }

        return item;
    }

    public User? GetByName(string name)
    {
        User? item;
        using (var db = new AppContext())
        {
            item = db.Users.Where(x => x.Name == name).FirstOrDefault();
        }

        return item;
    }

    public void Add(params User[] item)
    {
        using (var db = new AppContext())
        {
            db.Users.AddRange(item);

            db.SaveChanges();
        }
    }

    public void Remove(params User[] item)
    {
        using (var db = new AppContext())
        {
            db.Users.RemoveRange(item);

            db.SaveChanges();
        }
    }

    public void RemoveById(int Id)
    {
        using (var db = new AppContext())
        {
            db.Users.RemoveRange(db.Users.Where(x => x.Id == Id));

            db.SaveChanges();
        }
    }

    public void UpdateNameById(int Id, string name)
    {
        using (var db = new AppContext())
        {
            User? item;
            item = db.Users.Where(x => x.Id == Id).FirstOrDefault();
            if (item is User user)
            {
                user.Name = name;

                db.SaveChanges();
            }
        }
    }
}
