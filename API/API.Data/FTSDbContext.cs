using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Data
{
    public class FTSDbContext : DbContext
    {
        public FTSDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
