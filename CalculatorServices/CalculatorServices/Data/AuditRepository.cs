﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CalculatorServices.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace CalculatorServices.Data
{
    public class AuditRepository<T> : DbContext, IAuditRepository<T>
        where T : BaseOperationAuditRecord, new()
    {
        public DbSet<T> Audits { get; set; }

        public AuditRepository(DbContextOptions<AuditRepository<T>> options)
            : base(options)
        { }

        public async Task AddAudit(T auditRecord, CancellationToken cancellationToken)
        {
            this.Audits.Add(auditRecord);
            await this.SaveChangesAsync(cancellationToken);
        }
    }
}
