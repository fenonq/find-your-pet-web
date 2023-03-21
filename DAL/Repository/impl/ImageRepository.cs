using DAL.DataContext;
using DAL.Model;

namespace DAL.Repository.impl;

public class ImageRepository : IEntityRepository<Image>
{
    private readonly FindYourPetContext _context;

    public ImageRepository(FindYourPetContext context)
    {
        _context = context;
    }

    public IQueryable<Image> FindAll()
    {
        return _context.Images;
    }

    public Image? FindById(int id)
    {
        return _context.Images.Find(id);
    }

    public void Add(Image image)
    {
        _context.Images.Add(image);
        _context.SaveChanges();
    }

    public void Update(Image image)
    {
        _context.Images.Update(image);
        _context.SaveChanges();
    }

    public void Remove(int id)
    {
        var image = _context.Images.Find(id);

        if (image == null) return;
        _context.Images.Remove(image);
        _context.SaveChanges();
    }
}