using ApiObrasBibliograficas.Data;
using ApiObrasBibliograficas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiObrasBibliograficas.Services
{

    public class AuthorServices
    {
        private readonly ApiContext _context;

        public AuthorServices(ApiContext context)
        {
            _context = context;
        }

        public void AdicionarAuthor(Author author)
        {          
            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        public IEnumerable<Author> GetAll()
        {
            return _context.Authors.ToArray();
        }

    }
}
