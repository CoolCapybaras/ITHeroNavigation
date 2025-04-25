using Domain.Models;

namespace Domain.Interfaces;

public interface IPhotoRepository
{
    public Task AddPhotoAsync(Photo photo);

    public Task<List<Photo>> GetPhotosAsync(Guid placeId, int offset, int count);
}
