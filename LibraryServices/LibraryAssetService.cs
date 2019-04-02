using System;
using System.Collections.Generic;
using System.Linq;
using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryServices
{
    public class LibraryAssetService : ILibraryAsset
    {
        private readonly LibraryContext _context;

        public LibraryAssetService(LibraryContext context)
        {
            _context = context;
        }

        public IEnumerable<LibraryAsset> GetAll()
        {
            return _context.LibraryAssets
                .Include(asset => asset.Status)
                .Include(asset => asset.Location)
                .AsNoTracking();
        }

        public LibraryAsset GetById(int id)
        {
            return GetAll()
                .FirstOrDefault(asset => asset.Id == id);
        }

        public void Add(LibraryAsset newAsset)
        {
            _context.Add(newAsset);
            _context.SaveChanges();
        }

        public string GetAuthorOrDirector(int id)
        {
            var bookToCheck = _context.Books.FirstOrDefault(book => book.Id == id);
            if (bookToCheck == null)
            {
                return _context.Videos
                    .FirstOrDefault(video => video.Id == id)?.Director;
            }

            return bookToCheck.Author;
        }

        public string GetDeweyIndex(int id)
        {
            return _context
                .Books
                .FirstOrDefault(book => book.Id == id)?
                .DeweyIndex;
        }

        public string GetType(int id)
        {
            if(_context.Books.Any(book => book.Id == id))
            {
                return "Book";
            }
            if (_context.Videos.Any(video => video.Id == id))
            {
                return "Video";
            }

            return "Unknown";
        }

        public string GetTitle(int id)
        {
            return _context.LibraryAssets
                .FirstOrDefault(asset => asset.Id == id)?
                .Title;
        }

        public string GetIsbn(int id)
        {
            return _context
                .Books
                .FirstOrDefault(book => book.Id == id)?
                .ISBN;
        }

        public LibraryBranch GetCurrentLocation(int id)
        {
            return GetById(id).Location;
        }
    }
}
