using System;
using System.Collections.Generic;
using System.IO;
using GitTagApp.Entities;
using GitTagApp.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;

namespace GitTagApp.Tests.Unit.Repositories.Context
{
    public class FakeContext
    {
        public FakeContext(string testName)
        {
            FakeOptions = new DbContextOptionsBuilder<MainContext>()
                .UseInMemoryDatabase($"GitTagApp_{testName}")
                .Options;

            var path = @$"{AppDomain.CurrentDomain.BaseDirectory.Split(@"\bin")[0]}\FakeData\";

            DataFileNames.Add(typeof(User), $"{path}users.json");
            DataFileNames.Add(typeof(GitRepo), $"{path}repos.json");
            DataFileNames.Add(typeof(GitRepoTag), $"{path}repotag.json");
            DataFileNames.Add(typeof(Tag), $"{path}tags.json");
        }

        public DbContextOptions<MainContext> FakeOptions { get; }

        private Dictionary<Type, string> DataFileNames { get; } =
            new Dictionary<Type, string>();

        private string FileName<T>()
        {
            return DataFileNames[typeof(T)];
        }

        public void FillWithAll()
        {
            FillWith<User>();
            FillWith<Tag>();
            FillWith<GitRepoTag>();
            FillWith<GitRepo>();
        }

        public void FillWith<T>() where T : class
        {
            using (var context = new MainContext(FakeOptions))
            {
                if (context.Set<T>().Any()) return;
                foreach (var item in GetFakeData<T>())
                    context.Set<T>().Add(item);
                context.SaveChanges();
            }
        }

        public List<T> GetFakeData<T>()
        {
            var content = File.ReadAllText(FileName<T>());
            return JsonConvert.DeserializeObject<List<T>>(content);
        }
    }
}