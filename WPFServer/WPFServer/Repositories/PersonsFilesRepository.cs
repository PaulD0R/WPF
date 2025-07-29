using Microsoft.EntityFrameworkCore;
using WPFServer.Context;
using WPFServer.Interfaces;
using WPFServer.Models;
namespace WPFServer.Repositories
{
    public class PersonsFilesRepository(ApplicationContext context) : IPersonsFilesRepository
    {
        private readonly ApplicationContext _context = context;

        public async Task<PersonsFiles?> ChangeImageAsync(string personId, byte[] image)
        {
            var files = await _context.PersonsFiles.FirstOrDefaultAsync(x => x.PersonId ==  personId);

            if (files == null) return null;

            files.Image = image;

            await _context.SaveChangesAsync();
            return files;
        }

        public async Task<PersonsFiles> CreateNewAsync(string personId)
        {
            var personsFiles = new PersonsFiles
            {
                PersonId = personId,
                Image = null
            };

            await _context.PersonsFiles.AddAsync(personsFiles);
            await _context.SaveChangesAsync();

            return personsFiles;    
        }

        public async Task<PersonsFiles?> DeleteImageAsync(string personId)
        {
            var files = await _context.PersonsFiles.FirstOrDefaultAsync(x => x.PersonId == personId);

            if (files == null) return null;

            files.Image = null;

            await _context.SaveChangesAsync();
            return files;
        }

        public async Task<byte[]?> GetPersonsImageByPersonIdAsync(string personId)
        {
            var files = await _context.PersonsFiles.FirstOrDefaultAsync(x => x.PersonId == personId);

            return files?.Image;
        }
    }
}
