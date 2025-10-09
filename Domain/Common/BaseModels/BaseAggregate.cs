using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Common.BaseModels;

public class BaseAggregate
{
    public Guid Id { get; protected set; }
    public DateTime LastModifiedDate { get; protected set; }
    public DateTime CreatedDate { get; protected set; }
    public bool IsDeleted { get; protected set; }
    public Guid? CreatedByUserId { get; protected set; }
    public Guid? UpdatedByUserId { get; protected set; }

    [JsonIgnore] public bool IsModified { get; protected set; }

    public void SetAsCreated()
    {
        var now = DateTime.UtcNow;
        Id = Guid.NewGuid();
        CreatedDate = now;
        LastModifiedDate = now;
        IsDeleted = false;
        IsModified = true;
        CreatedByUserId = Id;
    }

    public void SetAsModified(Guid? updatedByUserId = null)
    {
        LastModifiedDate = DateTime.UtcNow;
        IsModified = true;
        UpdatedByUserId = updatedByUserId.HasValue && updatedByUserId.Value != Guid.Empty
            ? updatedByUserId.Value
            : Guid.Empty;
    }

    public void Delete()
    {
        IsDeleted = true;
        SetAsModified();
    }

    public void Restore()
    {
        if (!IsDeleted) return;
        IsDeleted = false;
        SetAsModified();
    }
}