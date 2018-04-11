using BsuirHealthProjectServer.Shared;
using BsuirHealthProjectServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace BsuirHealthProjectServer.Services
{
    public abstract class Service<T>
    {
        protected ApplicationDbContext context;

        public Service(ApplicationDbContext context)
        {
            this.context = context;
        }

        public abstract T Get(int id);

        public abstract IQueryable<T> Get();

        public abstract ValidationResult Add(T item);

        public abstract ValidationResult Update(int id, T item);

        public abstract ValidationResult Remove(int id);

        public abstract ValidationResult Remove(T item);

        public abstract ValidationResult IsCorrectItem(T item);

    }
}
