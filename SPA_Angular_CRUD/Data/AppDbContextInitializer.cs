using Newtonsoft.Json;
using SPA_Angular_CRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SPA_Angular_CRUD.Data
{
    public class AppDbContextInitializer : DropCreateDatabaseAlways<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            //loading positions
            if (!context.Positions.Any())
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Positions.json");
                using (StreamReader r = new StreamReader(new FileStream(path, FileMode.Open)))
                {
                    var positions = JsonConvert.DeserializeObject<List<Position>>(r.ReadToEnd()).Distinct();
                    context.Positions.AddRange(positions);
                    context.SaveChanges();
                }
            }

            //loading employees
            if (!context.Employees.Any())
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Employees.json");
                using (StreamReader r = new StreamReader(new FileStream(path, FileMode.Open)))
                {
                    var employees = JsonConvert.DeserializeObject<List<Employee>>(r.ReadToEnd()).Distinct();
                    context.Employees.AddRange(employees);
                    context.SaveChanges();
                }
            }

            base.Seed(context);
        }
    }
}