﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Acturis
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EclipseODSEntities : DbContext
    {
        public EclipseODSEntities()
            : base("name=EclipseODSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<tblClaim> tblClaims { get; set; }
        public DbSet<tblClaimMovement> tblClaimMovements { get; set; }
        public DbSet<tblClaimMovementBreakdown> tblClaimMovementBreakdowns { get; set; }
        public DbSet<tblPolicy> tblPolicies { get; set; }
        public DbSet<tblPolicySection> tblPolicySections { get; set; }
    }
}