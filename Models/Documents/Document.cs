using System;
using System.Collections.Generic;

namespace DocuIns.Models.Documents;

public partial class Document
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Path { get; set; }

    public string? UserId { get; set; }

    public string? Status { get; set; }

    public string? Tag { get; set; }

    public string? CreatedDate { get; set; }

    public string? ModifiedDate { get; set; }

    public virtual AspNetUser? User { get; set; }
}
