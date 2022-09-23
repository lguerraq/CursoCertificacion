using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DBR.Eventos.Datos.Base
{
    public class BaseAcceso : IDisposable
    {
        private DbContext _context;

        public BaseAcceso(DbContext context)
        {
            _context = context;
        }
        protected DbContext Context
        {
            get { return _context; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

        public Paged<T> Paginate<T, TKey>(PageRequest page, IQueryable<T> query, Expression<Func<T, TKey>> filter)
        {
            Paged<T> obj = new Paged<T>();
            int PageIndex = page.PageNumber - 1;
            var rowsTotal = query.ToList().Count();

            if (page.Order.ToUpper() == "DESC")
            {
                query = query.OrderByDescending(filter);
                query = query.Skip(PageIndex * page.PageSize).Take(page.PageSize);
            }
            else
            {
                query = query.OrderBy(filter);
                query = query.Skip(PageIndex * page.PageSize).Take(page.PageSize);
            }

            obj.data = query.ToList();
            obj.recordsTotal = rowsTotal;
            obj.recordsFiltered = rowsTotal;
            return obj;
        }
        public Paged<InscripcionResponse> PaginateAlt(PageRequest page, IQueryable<InscripcionResponse> query)
        {
            Paged<InscripcionResponse> obj = new Paged<InscripcionResponse>();
            int PageIndex = page.PageNumber - 1;
            var rowsTotal = query.ToList().Count();

            query = query.OrderByDescending(x => x.TipoInscripcion).ThenBy(x => x.ApellidoPaterno);
            query = query.Skip(PageIndex * page.PageSize).Take(page.PageSize);

            obj.data = query.ToList();
            obj.recordsTotal = rowsTotal;
            obj.recordsFiltered = rowsTotal;
            return obj;
        }
        public Paged<T> PaginateNew<T, TKey>(PageRequest page, IQueryable<T> query, Expression<Func<T, TKey>> filter)
        {
            Paged<T> obj = new Paged<T>();
            int PageIndex = page.PageNumber - 1;
            var rowsTotal = query.ToList().Count();

            page.Order = page.Order == null ? "ASC" : page.Order;

            if (page.Order.ToUpper() == "DESC")
            {
                query = query.OrderByDescending(filter);
                query = query.Skip(page.start).Take(page.length);
            }
            else
            {
                query = query.OrderBy(filter);
                query = query.Skip(page.start).Take(page.length);
            }

            obj.data = query.ToList();
            obj.recordsTotal = rowsTotal;
            obj.recordsFiltered = rowsTotal;
            return obj;
        }
    }
}
