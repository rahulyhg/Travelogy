//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DomingoDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class View_Thread
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorUserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime MostRecentPostDate { get; set; }
        public string Tags { get; set; }
        public int Type { get; set; }
        public string AspnetUserId { get; set; }
        public Nullable<int> tripthreadid { get; set; }
    }
}
