using System;
using System.Collections.Generic;
using System.Linq;
using CalculatorServices.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace CalculatorServices.Data
{
    public class CalculatorRepository : DbContext, ICalculatorRepository
    {
        public DbSet<AdditionAuditRecord> AdditionAudits { get; set; }

        public DbSet<SubtractionAuditRecord> SubtractionAudits { get; set; }

        public CalculatorRepository(DbContextOptions options)
            : base(options)
        { }
    }
}
