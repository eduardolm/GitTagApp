using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GitTagApp.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GitTagApp.Repositories.Context
{
    public class DbInitializer
    {
        public static DbContextOptions<MainContext> DbOptions { get; set; }
        
        private static Dictionary<Type, string> DataFileNames { get; } = new Dictionary<Type, string>();
        
        private static string FileName<T>()
        {
            return DataFileNames[typeof(T)];
        }
        public static void Initialize(MainContext context)
        {
            DbOptions = new DbContextOptionsBuilder<MainContext>()
                .Options;
            
            if (context.Database.EnsureCreated()) return;
            

            if (context.Tags.Any())
            {
                return;
            }
            
            DataFileNames.Add(typeof(User), $@"..\GitTagApp.Test\FakeData{Path.DirectorySeparatorChar}users.json");
            DataFileNames.Add(typeof(GitRepo), $@"..\GitTagApp.Test\FakeData{Path.DirectorySeparatorChar}repos.json");
            DataFileNames.Add(typeof(Tag), $@"..\GitTagApp.Test\FakeData{Path.DirectorySeparatorChar}tags.json");
            DataFileNames.Add(typeof(Tag), $@"..\GitTagApp.Test\FakeData{Path.DirectorySeparatorChar}repotag.json");
            FillWithAll();
        }
        
        public static void FillWithAll()
        {
            FillWith<User>();
            FillWith<GitRepo>();
            FillWith<Tag>();
        }
        
        public static void FillWith<T>() where T : class
        {
            using (var context = new MainContext(DbOptions))
            {
                if (context.Set<T>().Count() == 0)
                {
                    context.Database.OpenConnection();
                    foreach (var item in GetData<T>())
                    {
                        var fullName = item.GetType().FullName;
                        if (fullName != null)
                        {
                            context.Set<T>().Add(item);
                            context.SaveChanges();
                        }
                    }
                    context.Database.CloseConnection();
                }
            }
        }
        
        public static List<T> GetData<T>()
        {
            var content = File.ReadAllText(FileName<T>());
            return JsonConvert.DeserializeObject<List<T>>(content);
        }
    }
}