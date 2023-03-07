using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestJoin1.DataAccess;

public partial class TestJoinContext : DbContext
{
   
    public TestJoinContext(DbContextOptions<TestJoinContext> options)
        : base(options)
    {
    }

    public DbSet<Departamento> Departamentos { get; set; }

    public DbSet<Empleado> Empleados { get; set; }

     
}
