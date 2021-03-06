using Microsoft.EntityFrameworkCore;

using SmhiDb.Abstract;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SmhiDb
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly SmhiDbContext dbContext;
        //private readonly IMemoryCache cache;
        private readonly DbSet<TEntity> dbSet;

        public GenericRepository(SmhiDbContext dbContext/*, IMemoryCache cache*/)
        {
            this.dbContext = dbContext;
            //this.cache = cache;
            dbSet = dbContext.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }

        }

        public bool Exists(Expression<Func<TEntity, bool>> searchFor)
        {
            return dbSet.Any(searchFor);

            //var myLines = await
            //cache.GetOrCreateAsync("mykey", async entry => {
            //    return
            //        await _context.MyEntity.Where(somecondition).ToListAsync();
            //});
        }

        public TEntity GetByID(object id) => dbSet.Find(id);

        public void Insert(TEntity entity) => dbSet.Add(entity);

        public async Task InsertAsync(TEntity entity) => await dbSet.AddAsync(entity);

        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void DeleteByID(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            if (dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }

            dbSet.Remove(entityToDelete);
        }
    }
}
