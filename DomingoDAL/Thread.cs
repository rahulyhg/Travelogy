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
    
    public partial class Thread
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorUserId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int ModerationStatus { get; set; }
        public int SafeFilterStatus { get; set; }
        public System.DateTime MostRecentPostDate { get; set; }
        public int MostRecentUserId { get; set; }
        public int TotalViews { get; set; }
        public int TotalReplies { get; set; }
        public string Tags { get; set; }
        public bool IsPrivate { get; set; }
        public bool AllowInvites { get; set; }
        public int Type { get; set; }
        public byte[] ts { get; set; }
        public Nullable<System.DateTime> FlaggedOn { get; set; }
        public Nullable<int> LastMessageId { get; set; }
    }
}
