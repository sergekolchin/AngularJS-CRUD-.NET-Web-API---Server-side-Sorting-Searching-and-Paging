using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPA_Angular_CRUD.Models
{
    public class DataWrapper<T>
    {
        public int TotalItems { get; set; }
        public IQueryable<T> Data { get; set; }
    }
}