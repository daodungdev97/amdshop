using Ecom.DataAccess.Data;
using Ecom.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
            _db.Products.Include(u=>u.Category).Include(u => u.CategoryId);
        
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }
        //tham số T, Result bool, đối số filter
        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        //includeProperties cho phép bạn chỉ định các thuộc tính liên quan mà bạn muốn tải lười biếng khi trả về đối tượng.
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = dbSet;

            }
            else
            {
                query = dbSet.AsNoTracking();
            }

            query = query.Where(filter);
            //phân tách chuỗi  includeProperties bằng dấu ,lặp qua từng thuộc tính, Include sẽ gọi thuộc tính cần tải lên
            //kiểm tra và tải lười biếng được chỉ định trong includeProperties,đảm bảo   thuộc tính liên quan cũng được tải lên và có sẵn để sử dụng mà không cần thực hiện các truy vấn riêng biệt.
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();

        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query = dbSet;

            if (tracked)
            {
                query = dbSet;

            }
            else
            {
                query = dbSet.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
         
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
