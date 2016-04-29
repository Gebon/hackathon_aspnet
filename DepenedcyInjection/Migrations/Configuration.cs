using Domain;

namespace DepenedcyInjection.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DependencyInjection.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DependencyInjection.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Characters.AddOrUpdate(
                p => p.Id,
                new Character {Born = "In 103 DC", Died = false, Gender = Gender.Male, Name = "Eddard Stark", Id = 1,Cost = 10},
                new Character {Born = "In 102 DC", Died = true, Gender = Gender.Female, Name = "Sylwa Paege", Id = 2, Cost = 2},
                new Character {Born = "In 105 DC", Died = false, Gender = Gender.Male, Name = "Terrence Toyne", Id = 3, Cost = 7},
                new Character {Born = "In 101 DC", Died = false, Gender = Gender.Female, Name = "Tanda Stokeworth", Id = 4, Cost = 3});
        }
    }
}
