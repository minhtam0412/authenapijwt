//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimpleAuthen
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClientMaster
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ClientName { get; set; }
        public Nullable<decimal> RefreshTokenLifeTime { get; set; }
        public string AllowedOrigin { get; set; }
        public Nullable<bool> Active { get; set; }
        public int ClientKeyId { get; set; }
    }
}
