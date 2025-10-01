using Microsoft.EntityFrameworkCore;
using WPFServer.Context;
using WPFServer.Interfaces.Repositories;
using WPFServer.Models;

namespace WPFServer.Repositories
{
    public class PersonsFilesRepository(ApplicationContext context) : IPersonsFilesRepository
    {
        public async Task<PersonsFiles?> ChangeImageAsync(string personId, byte[]? image)
        {
            var files = await context.PersonsFiles.FirstOrDefaultAsync(x => x.PersonId == personId);
            if (files == null) return null;

            files.Image = image;

            await context.SaveChangesAsync();
            return files;
        }

        public async Task<PersonsFiles> CreateNewAsync(string personId)
        {
            var personsFiles = new PersonsFiles
            {
                PersonId = personId,
                Image = null
            };

            await context.PersonsFiles.AddAsync(personsFiles);
            await context.SaveChangesAsync();

            return personsFiles;    
        }

        public async Task<PersonsFiles?> DeleteImageAsync(string personId)
        {
            var files = await context.PersonsFiles.FirstOrDefaultAsync(x => x.PersonId == personId);

            if (files == null) return null;

            files.Image = null;

            await context.SaveChangesAsync();
            return files;
        }

        public async Task<byte[]?> GetPersonsImageByPersonIdAsync(string personId)
        {
            var files = await context.PersonsFiles.FirstOrDefaultAsync(x => x.PersonId == personId);

            return files?.Image;
        }
    }
}
