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
        await DbSet.InsertOneAsync(obj);
    }

    public virtual async Task<TDocument> GetById(Guid id)
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
        Context.AddCommand(() => DbSet.ReplaceOneAsync(Builders<TDocument>.Filter.Eq("_id", obj.Id), obj));
    }

    public virtual async Task Remove(ObjectId id)
    {
        Context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TDocument>.Filter.Eq("_id", id)));
    }

    public async Task Remove(TDocument obj)
    {
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
