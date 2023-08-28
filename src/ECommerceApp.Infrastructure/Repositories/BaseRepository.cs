using ECommerceApp.Core.Domain;
using ECommerceApp.Core.Domain.Interfaces;
using ECommerceApp.Core.Domain.Interfaces.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections;


namespace ECommerceApp.Infrastructure.Repositories;

public class BaseRepository<TDocument> : IRepository<TDocument, ObjectId> where TDocument : Document
{
    protected readonly IMongoContext Context;
    protected IMongoCollection<TDocument> DbSet;

    public BaseRepository(IMongoContext context)
    {
        Context = context;

        DbSet = Context.GetCollection<TDocument>(typeof(TDocument).Name);
    }

    public virtual async Task Add(TDocument obj)
    {
        if (typeof(TDocument).IsAssignableTo(typeof(DocumentShortTrack))
            || typeof(TDocument).IsAssignableTo(typeof(DocumentLongTrack)))
        {
            var documentAsShortTrack = obj as DocumentShortTrack;
            documentAsShortTrack.CreatedDate = DateTime.UtcNow;
            documentAsShortTrack.CreatedUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }
        await DbSet.InsertOneAsync(obj);
    }

    public virtual async Task<TDocument> GetById(ObjectId id)
    {
        var data = await DbSet.FindAsync(Builders<TDocument>.Filter.Eq("_id", id));
        return data.SingleOrDefault();
    }

    public virtual IQueryable<TDocument> GetAll()
    {
        return DbSet.AsQueryable();
    }

    public virtual async Task Update(TDocument obj)
    {
        if (typeof(TDocument).IsAssignableTo(typeof(DocumentLongTrack)))
        {
            var documentAsLongTrack = obj as DocumentLongTrack;
            documentAsLongTrack.UpdatedDate = DateTime.UtcNow;
            documentAsLongTrack.UpdatedUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }
        Context.AddCommand(() => DbSet.ReplaceOneAsync(Builders<TDocument>.Filter.Eq("_id", obj.Id), obj));
    }

    public virtual async Task Remove(ObjectId id)
    {
        if (typeof(TDocument).IsAssignableTo(typeof(DocumentLongTrack)))
        {
            var obj = await GetById(id);
            var documentAsLongTrack = obj as DocumentLongTrack;
            documentAsLongTrack.UpdatedDate = DateTime.UtcNow;
            documentAsLongTrack.UpdatedUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            documentAsLongTrack.IsDeleted = true;
            Context.AddCommand(async () => await DbSet.ReplaceOneAsync(Builders<TDocument>.Filter.Eq("_id", obj.Id), obj));
        }
        else
            Context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TDocument>.Filter.Eq("_id", id)));
    }

    public async Task Remove(TDocument obj)
    {
        if (typeof(TDocument).IsAssignableTo(typeof(DocumentLongTrack)) && obj is DocumentLongTrack documentAsLongTrack)
        {
            documentAsLongTrack.UpdatedDate = DateTime.UtcNow;
            documentAsLongTrack.UpdatedUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            documentAsLongTrack.IsDeleted = true;
            await DbSet.ReplaceOneAsync(Builders<TDocument>.Filter.Eq("_id", obj.Id), obj);
        }
        else
            await DbSet.DeleteOneAsync(e => e.Id == obj.Id);
    }

    public async Task SaveChanges()
    {
        await Context.SaveChanges();
    }

    public void Dispose()
    {
        Context?.Dispose();
    }
}
