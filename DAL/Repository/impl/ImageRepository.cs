namespace DAL.Repository.impl;

using DataContext;
using Model;

public class ImageRepository : IEntityRepository<Image>
{
    private readonly FindYourPetContext _context;

    public ImageRepository(FindYourPetContext context)
    {
        this._context = context;
    }

    public IQueryable<Image> FindAll()
    {
        return this._context.Images;
    }

    public Image? FindById(int id)
    {
        return this._context.Images.Find(id);
    }

    public void Add(Image image)
    {
        this._context.Images.Add(image);
        this._context.SaveChanges();
    }

    public void Update(Image image)
    {
        this._context.Images.Update(image);
        this._context.SaveChanges();
    }

    public void Remove(int id)
    {
        var image = this._context.Images.Find(id);

        if (image == null) return;
        this._context.Images.Remove(image);
        this._context.SaveChanges();
    }
}