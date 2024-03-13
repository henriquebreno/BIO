using BIO_API_DATA.API_Client.Interfaces.Database;
using BIO_API_DATA.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIO_API_DATA.API_Client.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BioDataContext _context;

        public UnitOfWork(BioDataContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
